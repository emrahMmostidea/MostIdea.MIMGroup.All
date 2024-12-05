using Abp.AspNetCore.Mvc.ViewComponents;

namespace MostIdea.MIMGroup.Web.Views
{
    public abstract class MIMGroupViewComponent : AbpViewComponent
    {
        protected MIMGroupViewComponent()
        {
            LocalizationSourceName = MIMGroupConsts.LocalizationSourceName;
        }
    }
}