using Abp.Authorization;
using MostIdea.MIMGroup.Authorization.Roles;
using MostIdea.MIMGroup.Authorization.Users;

namespace MostIdea.MIMGroup.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
