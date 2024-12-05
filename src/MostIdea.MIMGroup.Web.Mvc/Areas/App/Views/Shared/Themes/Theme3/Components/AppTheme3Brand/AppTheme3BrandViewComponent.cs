using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.Layout;
using MostIdea.MIMGroup.Web.Session;
using MostIdea.MIMGroup.Web.Views;

namespace MostIdea.MIMGroup.Web.Areas.App.Views.Shared.Themes.Theme3.Components.AppTheme3Brand
{
    public class AppTheme3BrandViewComponent : MIMGroupViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme3BrandViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var headerModel = new HeaderViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(headerModel);
        }
    }
}
