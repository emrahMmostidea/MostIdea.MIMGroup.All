using System.Threading.Tasks;
using Abp.Authorization.Users;
using MostIdea.MIMGroup.Authorization.Users;

namespace MostIdea.MIMGroup.Authorization
{
    public static class UserManagerExtensions
    {
        public static async Task<User> GetAdminAsync(this UserManager userManager)
        {
            return await userManager.FindByNameAsync(AbpUserBase.AdminUserName);
        }
    }
}
