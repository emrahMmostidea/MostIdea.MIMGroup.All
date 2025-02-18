﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using MostIdea.MIMGroup.Authentication.TwoFactor;
using MostIdea.MIMGroup.Editions;
using MostIdea.MIMGroup.MultiTenancy.Payments;
using MostIdea.MIMGroup.Sessions.Dto;
using MostIdea.MIMGroup.UiCustomization;
using MostIdea.MIMGroup.Authorization.Delegation;
using MostIdea.MIMGroup.Authorization.Users;
using Abp.Domain.Uow;

namespace MostIdea.MIMGroup.Sessions
{
    public class SessionAppService : MIMGroupAppServiceBase, ISessionAppService
    {
        private readonly IUiThemeCustomizerFactory _uiThemeCustomizerFactory;
        private readonly ISubscriptionPaymentRepository _subscriptionPaymentRepository;
        private readonly IUserDelegationConfiguration _userDelegationConfiguration;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public SessionAppService(
            IUiThemeCustomizerFactory uiThemeCustomizerFactory,
            ISubscriptionPaymentRepository subscriptionPaymentRepository,
            IUserDelegationConfiguration userDelegationConfiguration,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _uiThemeCustomizerFactory = uiThemeCustomizerFactory;
            _subscriptionPaymentRepository = subscriptionPaymentRepository;
            _userDelegationConfiguration = userDelegationConfiguration;
            _unitOfWorkManager = unitOfWorkManager;
        }

        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            return await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                var output = new GetCurrentLoginInformationsOutput
                {
                    Application = new ApplicationInfoDto
                    {
                        Version = AppVersionHelper.Version,
                        ReleaseDate = AppVersionHelper.ReleaseDate,
                        Features = new Dictionary<string, bool>(),
                        Currency = MIMGroupConsts.Currency,
                        CurrencySign = MIMGroupConsts.CurrencySign,
                        AllowTenantsToChangeEmailSettings = MIMGroupConsts.AllowTenantsToChangeEmailSettings,
                        UserDelegationIsEnabled = _userDelegationConfiguration.IsEnabled,
                        TwoFactorCodeExpireSeconds = TwoFactorCodeCacheItem.DefaultSlidingExpireTime.TotalSeconds
                    }
                };

                var uiCustomizer = await _uiThemeCustomizerFactory.GetCurrentUiCustomizer();
                output.Theme = await uiCustomizer.GetUiSettings();

                if (AbpSession.TenantId.HasValue)
                {
                    output.Tenant = ObjectMapper
                        .Map<TenantLoginInfoDto>(await TenantManager
                            .Tenants
                            .Include(t => t.Edition)
                            .FirstAsync(t => t.Id == AbpSession.GetTenantId()));
                }

                if (AbpSession.ImpersonatorTenantId.HasValue)
                {
                    output.ImpersonatorTenant = ObjectMapper
                        .Map<TenantLoginInfoDto>(await TenantManager
                            .Tenants
                            .Include(t => t.Edition)
                            .FirstAsync(t => t.Id == AbpSession.ImpersonatorTenantId));
                }

                if (AbpSession.UserId.HasValue)
                {
                    output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
                }

                if (AbpSession.ImpersonatorUserId.HasValue)
                {
                    output.ImpersonatorUser = ObjectMapper.Map<UserLoginInfoDto>(await GetImpersonatorUserAsync());
                }

                if (output.Tenant == null)
                {
                    return output;
                }

                if (output.Tenant.Edition != null)
                {
                    var lastPayment = await _subscriptionPaymentRepository.GetLastCompletedPaymentOrDefaultAsync(output.Tenant.Id, null, null);
                    if (lastPayment != null)
                    {
                        output.Tenant.Edition.IsHighestEdition = IsEditionHighest(output.Tenant.Edition.Id, lastPayment.GetPaymentPeriodType());
                    }
                }

                output.Tenant.SubscriptionDateString = GetTenantSubscriptionDateString(output);
                output.Tenant.CreationTimeString = output.Tenant.CreationTime.ToString("d");

                return output;
            });

        }

        private bool IsEditionHighest(int editionId, PaymentPeriodType paymentPeriodType)
        {
            var topEdition = GetHighestEditionOrNullByPaymentPeriodType(paymentPeriodType);
            if (topEdition == null)
            {
                return false;
            }

            return editionId == topEdition.Id;
        }

        private SubscribableEdition GetHighestEditionOrNullByPaymentPeriodType(PaymentPeriodType paymentPeriodType)
        {
            var editions = TenantManager.EditionManager.Editions;
            if (editions == null || !editions.Any())
            {
                return null;
            }

            var query = editions.Cast<SubscribableEdition>();

            switch (paymentPeriodType)
            {
                case PaymentPeriodType.Daily:
                    query = query.OrderByDescending(e => e.DailyPrice ?? 0); break;
                case PaymentPeriodType.Weekly:
                    query = query.OrderByDescending(e => e.WeeklyPrice ?? 0); break;
                case PaymentPeriodType.Monthly:
                    query = query.OrderByDescending(e => e.MonthlyPrice ?? 0); break;
                case PaymentPeriodType.Annual:
                    query = query.OrderByDescending(e => e.AnnualPrice ?? 0); break;
            }

            return query.FirstOrDefault();
        }

        private string GetTenantSubscriptionDateString(GetCurrentLoginInformationsOutput output)
        {
            return output.Tenant.SubscriptionEndDateUtc == null
                ? L("Unlimited")
                : output.Tenant.SubscriptionEndDateUtc?.ToString("d");
        }

        public async Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken()
        {
            if (AbpSession.UserId <= 0)
            {
                throw new Exception(L("ThereIsNoLoggedInUser"));
            }

            var user = await UserManager.GetUserAsync(AbpSession.ToUserIdentifier());
            user.SetSignInToken();
            return new UpdateUserSignInTokenOutput
            {
                SignInToken = user.SignInToken,
                EncodedUserId = Convert.ToBase64String(Encoding.UTF8.GetBytes(user.Id.ToString())),
                EncodedTenantId = user.TenantId.HasValue
                    ? Convert.ToBase64String(Encoding.UTF8.GetBytes(user.TenantId.Value.ToString()))
                    : ""
            };
        }
        
        protected virtual async Task<User> GetImpersonatorUserAsync()
        {
            using (CurrentUnitOfWork.SetTenantId(AbpSession.ImpersonatorTenantId))
            {
                var user = await UserManager.FindByIdAsync(AbpSession.ImpersonatorUserId.ToString());
                if (user == null)
                {
                    throw new Exception("User not found!");
                }

                return user;
            }
        }
    }
}
