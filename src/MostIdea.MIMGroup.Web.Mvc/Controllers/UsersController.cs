using Abp.AspNetCore.Mvc.Authorization;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.Storage;
using Abp.BackgroundJobs;
using Abp.Authorization;

namespace MostIdea.MIMGroup.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}