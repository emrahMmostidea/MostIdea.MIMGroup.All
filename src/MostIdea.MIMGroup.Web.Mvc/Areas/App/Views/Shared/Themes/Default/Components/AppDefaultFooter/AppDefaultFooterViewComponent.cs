using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.Layout;
using MostIdea.MIMGroup.Web.Session;
using MostIdea.MIMGroup.Web.Views;

namespace MostIdea.MIMGroup.Web.Areas.App.Views.Shared.Themes.Default.Components.AppDefaultFooter
{
    public class AppDefaultFooterViewComponent : MIMGroupViewComponent
    {
        private readonly IPerRequestSessionCache _sessionCache;

        public AppDefaultFooterViewComponent(IPerRequestSessionCache sessionCache)
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
