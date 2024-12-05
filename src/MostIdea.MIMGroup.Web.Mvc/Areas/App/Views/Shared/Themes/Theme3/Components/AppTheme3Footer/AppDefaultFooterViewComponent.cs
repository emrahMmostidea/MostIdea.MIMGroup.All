using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.Layout;
using MostIdea.MIMGroup.Web.Session;
using MostIdea.MIMGroup.Web.Views;

namespace MostIdea.MIMGroup.Web.Areas.App.Views.Shared.Themes.Theme3.Components.AppTheme3Footer
{
    public class AppTheme3FooterViewComponent : MIMGroupViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppTheme3FooterViewComponent(IPerRequestSessionCache sessionCache)
        {
            _sessionCache = sessionCache;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var footerModel = new FooterViewModel
            {
                LoginInformations = await _sessionCache.GetCurrentLoginInformationsAsync()
            };

            return View(footerModel);
        }
    }
}
