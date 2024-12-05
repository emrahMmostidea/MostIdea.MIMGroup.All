using Abp.AspNetCore.Mvc.Views;

namespace MostIdea.MIMGroup.Web.Views
{
    public abstract class MIMGroupRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected MIMGroupRazorPage()
        {
            LocalizationSourceName = MIMGroupConsts.LocalizationSourceName;
        }
    }
}
