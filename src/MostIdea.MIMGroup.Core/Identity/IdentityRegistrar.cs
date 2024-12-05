using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MostIdea.MIMGroup.Authentication.TwoFactor.Google;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.Authorization.Roles;
using MostIdea.MIMGroup.Authorization.Users;
using MostIdea.MIMGroup.Editions;
using MostIdea.MIMGroup.MultiTenancy;

namespace MostIdea.MIMGroup.Identity
{
    public static class IdentityRegistrar
    {
        public static IdentityBuilder Register(IServiceCollection services)
        {
            services.AddLogging();

            return services.AddAbpIdentity<Tenant, User, Role>(options =>
                {
                    options.Tokens.ProviderMap[GoogleAuthenticatorProvider.Name] = new TokenProviderDescriptor(typeof(GoogleAuthenticatorProvider));
                })
                .AddAbpTenantManager<TenantManager>()
                .AddAbpUserManager<UserManager>()
                .AddAbpRoleManager<RoleManager>()
                .AddAbpEditionManager<EditionManager>()
                .AddAbpUserStore<UserStore>()
                .AddAbpRoleStore<RoleStore>()
                .AddAbpSignInManager<SignInManager>()
                .AddAbpUserClaimsPrincipalFactory<UserClaimsPrincipalFactory>()
                .AddAbpSecurityStampValidator<SecurityStampValidator>()
                .AddPermissionChecker<PermissionChecker>()
                .AddDefaultTokenProviders();
        }
    }
}
