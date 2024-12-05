using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Identity;
using MostIdea.MIMGroup.Authorization.Accounts.Dto;
using MostIdea.MIMGroup.Authorization.Delegation;
using MostIdea.MIMGroup.Authorization.Impersonation;
using MostIdea.MIMGroup.Authorization.Users;
using MostIdea.MIMGroup.Authorization.Users.Dto;
using MostIdea.MIMGroup.B2B;
using MostIdea.MIMGroup.Configuration;
using MostIdea.MIMGroup.MultiTenancy;
using MostIdea.MIMGroup.Security.Recaptcha;
using MostIdea.MIMGroup.Url;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace MostIdea.MIMGroup.Authorization.Accounts
{
    /// <summary>
    ///
    /// </summary>
    public class AccountAppService : MIMGroupAppServiceBase, IAccountAppService
    {
        public IAppUrlService AppUrlService { get; set; }
        public IPermissionChecker PermissionChecker { get; set; }
        public IRecaptchaValidator RecaptchaValidator { get; set; }

        private readonly IUserEmailer _userEmailer;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly IImpersonationManager _impersonationManager;
        private readonly IUserLinkManager _userLinkManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IWebUrlService _webUrlService;
        private readonly IUserDelegationManager _userDelegationManager;
        private readonly IRepository<Hospital, Guid> _hospitalRepository;
        private readonly IRepository<HospitalVsUser, Guid> _hospitalVsUserRepository;
        private readonly IRepository<HospitalGroup, Guid> _hospitalGroupRepository;
        private readonly IRepository<DealerVsUser, Guid> _dealerVsUsersRepository;

        public AccountAppService(
                IUserEmailer userEmailer,
                UserRegistrationManager userRegistrationManager,
                IImpersonationManager impersonationManager,
                IUserLinkManager userLinkManager,
                IPasswordHasher<User> passwordHasher,
                IWebUrlService webUrlService,
                IUserDelegationManager userDelegationManager,
                IRepository<Hospital, Guid> hospitalRepository,
                IRepository<HospitalVsUser, Guid> hospitalVsUserRepository,
                IRepository<HospitalGroup, Guid> hospitalGroupRepository,
                IRepository<DealerVsUser, Guid> dealerVsUsersRepository)
        {
            _userEmailer = userEmailer;
            _userRegistrationManager = userRegistrationManager;
            _impersonationManager = impersonationManager;
            _userLinkManager = userLinkManager;
            _passwordHasher = passwordHasher;
            _webUrlService = webUrlService;
            _userDelegationManager = userDelegationManager;
            _hospitalRepository = hospitalRepository;
            _hospitalVsUserRepository = hospitalVsUserRepository;
            _hospitalGroupRepository = hospitalGroupRepository;
            _dealerVsUsersRepository = dealerVsUsersRepository;
        }

        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id, _webUrlService.GetServerRootAddress(input.TenancyName));
        }

        public Task<int?> ResolveTenantId(ResolveTenantIdInput input)
        {
            if (string.IsNullOrEmpty(input.c))
            {
                return Task.FromResult(AbpSession.TenantId);
            }

            var parameters = SimpleStringCipher.Instance.Decrypt(input.c);
            var query = HttpUtility.ParseQueryString(parameters);

            if (query["tenantId"] == null)
            {
                return Task.FromResult<int?>(null);
            }

            var tenantId = Convert.ToInt32(query["tenantId"]) as int?;
            return Task.FromResult(tenantId);
        }

        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            if (UseCaptchaOnRegistration())
            {
                await RecaptchaValidator.ValidateAsync(input.CaptchaResponse);
            }

            var user = await _userRegistrationManager.RegisterAsync(
                input.Name,
                input.Surname,
                input.EmailAddress,
                input.UserName,
                input.Password,
                false,
                AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId)
            );

            if (input.UserType == UserTypeEnum.Doctor && input.HospitalId != Guid.Empty)
            {
                var hospital = await _hospitalRepository.FirstOrDefaultAsync(input.HospitalId.Value);
                if (hospital == null) throw new UserFriendlyException(L("HospitalNotValid"));

                user.DiplomaNo = input.DiplomaNo;

                var hospitalVsUser = await _hospitalVsUserRepository.InsertAsync(new HospitalVsUser()
                {
                    HospitalId = input.HospitalId.Value,
                    UserId = user.Id
                });

                await CurrentUnitOfWork.SaveChangesAsync();
            }
            else if (input.UserType == UserTypeEnum.Doctor && input.HospitalId == Guid.Empty)
            {
                var hospitalGroup = await _hospitalGroupRepository.FirstOrDefaultAsync(x => x.Name == "Klinik");
                if (hospitalGroup == null)
                {
                    hospitalGroup = await _hospitalGroupRepository.InsertAsync(new HospitalGroup() { Name = "Klinik" });
                }

                var hospital = await _hospitalRepository.InsertAsync(new Hospital()
                {
                    Name = input.CompanyName,
                    TaxAdministration = input.TaxAdministration,
                    TaxNumber = input.TaxNumber,
                    HospitalGroupId = hospitalGroup.Id,
                });

                var hospitalVsUser = await _hospitalVsUserRepository.InsertAsync(new HospitalVsUser()
                { UserId = user.Id, HospitalId = hospital.Id });

                user.DiplomaNo = input.DiplomaNo;
                user.UserType = input.UserType;
            }
            else if (input.UserType == UserTypeEnum.Dealer)
            {
                var hospitalGroup = await _hospitalGroupRepository.FirstOrDefaultAsync(x => x.Name == "Bayi");
                if (hospitalGroup == null)
                {
                    hospitalGroup = await _hospitalGroupRepository.InsertAsync(new HospitalGroup() { Name = "Bayi" });
                }

                var hospital = await _hospitalRepository.InsertAsync(new Hospital()
                {
                    Name = input.CompanyName,
                    TaxAdministration = input.TaxAdministration,
                    TaxNumber = input.TaxNumber,
                    HospitalGroupId = hospitalGroup.Id,
                });

                var dealerVsUser = await _dealerVsUsersRepository.InsertAsync(new DealerVsUser()
                { UserId = user.Id, DealerId = hospital.Id });
            }

            var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

            return new RegisterOutput
            {
                CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin)
            };
        }

        public async Task SendPasswordResetCode(SendPasswordResetCodeInput input)
        {
            var user = await GetUserByChecking(input.EmailAddress);
            user.SetNewPasswordResetCode();
            await _userEmailer.SendPasswordResetLinkAsync(
                user,
                AppUrlService.CreatePasswordResetUrlFormat(AbpSession.TenantId)
                );
        }

        public async Task<ResetPasswordOutput> ResetPassword(ResetPasswordInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != input.ResetCode)
            {
                throw new UserFriendlyException(L("InvalidPasswordResetCode"), L("InvalidPasswordResetCode_Detail"));
            }

            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);
            CheckErrors(await UserManager.ChangePasswordAsync(user, input.Password));
            user.PasswordResetCode = null;
            user.IsEmailConfirmed = true;
            user.ShouldChangePasswordOnNextLogin = false;

            await UserManager.UpdateAsync(user);

            return new ResetPasswordOutput
            {
                CanLogin = user.IsActive,
                UserName = user.UserName
            };
        }

        public async Task SendEmailActivationLink(SendEmailActivationLinkInput input)
        {
            var user = await GetUserByChecking(input.EmailAddress);
            user.SetNewEmailConfirmationCode();
            await _userEmailer.SendEmailActivationLinkAsync(
                user,
                AppUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId)
            );
        }

        public async Task ActivateEmail(ActivateEmailInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            if (user != null && user.IsEmailConfirmed)
            {
                return;
            }

            if (user == null || user.EmailConfirmationCode.IsNullOrEmpty() || user.EmailConfirmationCode != input.ConfirmationCode)
            {
                throw new UserFriendlyException(L("InvalidEmailConfirmationCode"), L("InvalidEmailConfirmationCode_Detail"));
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationCode = null;

            await UserManager.UpdateAsync(user);
        }

        [AbpAuthorize(AppPermissions.Pages_Administration_Users_Impersonation)]
        public virtual async Task<ImpersonateOutput> Impersonate(ImpersonateInput input)
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetImpersonationToken(input.UserId, input.TenantId),
                TenancyName = await GetTenancyNameOrNullAsync(input.TenantId)
            };
        }

        public virtual async Task<ImpersonateOutput> DelegatedImpersonate(DelegatedImpersonateInput input)
        {
            var userDelegation = await _userDelegationManager.GetAsync(input.UserDelegationId);
            if (userDelegation.TargetUserId != AbpSession.GetUserId())
            {
                throw new UserFriendlyException("User delegation error.");
            }

            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetImpersonationToken(userDelegation.SourceUserId, userDelegation.TenantId),
                TenancyName = await GetTenancyNameOrNullAsync(userDelegation.TenantId)
            };
        }

        public virtual async Task<ImpersonateOutput> BackToImpersonator()
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetBackToImpersonatorToken(),
                TenancyName = await GetTenancyNameOrNullAsync(AbpSession.ImpersonatorTenantId)
            };
        }

        public virtual async Task<SwitchToLinkedAccountOutput> SwitchToLinkedAccount(SwitchToLinkedAccountInput input)
        {
            if (!await _userLinkManager.AreUsersLinked(AbpSession.ToUserIdentifier(), input.ToUserIdentifier()))
            {
                throw new Exception(L("This account is not linked to your account"));
            }

            return new SwitchToLinkedAccountOutput
            {
                SwitchAccountToken = await _userLinkManager.GetAccountSwitchToken(input.TargetUserId, input.TargetTenantId),
                TenancyName = await GetTenancyNameOrNullAsync(input.TargetTenantId)
            };
        }

        public virtual async Task<RegisterViewDto> GetRegisterViewModel()
        {
            var output = new RegisterViewDto();

            return output;
        }

        public async Task<bool> CheckPermission(string permissionName)
        {
            return await PermissionChecker.IsGrantedAsync(permissionName);
        }

        public List<string> GetAllPermissions()
        {
            var type = typeof(AppPermissions);
            return type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(AppPermissions))
            .Select(x => x.GetRawConstantValue().ToString())
            .ToList();

        }


            private bool UseCaptchaOnRegistration()
        {
            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnRegistration);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await TenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        private async Task<string> GetTenancyNameOrNullAsync(int? tenantId)
        {
            return tenantId.HasValue ? (await GetActiveTenantAsync(tenantId.Value)).TenancyName : null;
        }

        private async Task<User> GetUserByChecking(string inputEmailAddress)
        {
            var user = await UserManager.FindByEmailAsync(inputEmailAddress);
            if (user == null)
            {
                throw new UserFriendlyException(L("InvalidEmailAddress"));
            }

            return user;
        }
    }
}