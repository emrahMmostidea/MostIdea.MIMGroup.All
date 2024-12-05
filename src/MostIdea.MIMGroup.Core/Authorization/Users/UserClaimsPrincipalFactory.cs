using Abp.Authorization;
using Abp.Domain.Uow;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MostIdea.MIMGroup.Authorization.Roles;

namespace MostIdea.MIMGroup.Authorization.Users
{
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {
        public UserClaimsPrincipalFactory(
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            IUnitOfWorkManager unitOfWorkManager)
            : base(
                  userManager,
                  roleManager,
                  optionsAccessor,
                  unitOfWorkManager)
        {
            
        }
    }
}
