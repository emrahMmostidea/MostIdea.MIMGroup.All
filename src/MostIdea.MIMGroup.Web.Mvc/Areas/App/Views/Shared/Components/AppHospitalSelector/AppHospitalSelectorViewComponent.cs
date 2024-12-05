using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Areas.App.Models.Layout;
using MostIdea.MIMGroup.Web.Views;
using System.Threading.Tasks;

namespace MostIdea.MIMGroup.Web.Areas.App.Views.Shared.Components.AppHospitalSelector
{
    public class AppHospitalSelectorViewComponent : MIMGroupViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(string cssClass)
        {
            return Task.FromResult<IViewComponentResult>(View(new ChatTogglerViewModel
            {
                CssClass = cssClass
            }));
        }
    }
}
