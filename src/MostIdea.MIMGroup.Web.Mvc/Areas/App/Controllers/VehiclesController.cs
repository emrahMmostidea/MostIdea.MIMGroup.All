using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.Web.Controllers;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Vehicles)]
    public class VehiclesController : MIMGroupControllerBase
    {
        public VehiclesController()
        {
            
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}