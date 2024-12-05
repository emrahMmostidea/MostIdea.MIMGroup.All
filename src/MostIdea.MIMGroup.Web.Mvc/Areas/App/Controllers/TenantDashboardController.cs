using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.DashboardCustomization;
using System.Threading.Tasks;
using MostIdea.MIMGroup.Web.Areas.App.Startup;

namespace MostIdea.MIMGroup.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardController : CustomizableDashboardControllerBase
    {
        public TenantDashboardController(DashboardViewConfiguration dashboardViewConfiguration, 
            IDashboardCustomizationAppService dashboardCustomizationAppService) 
            : base(dashboardViewConfiguration, dashboardCustomizationAppService)
        {

        }

        public async Task<ActionResult> Index()
        {
            return await GetView(MIMGroupDashboardCustomizationConsts.DashboardNames.DefaultTenantDashboard);
        }
    }
}