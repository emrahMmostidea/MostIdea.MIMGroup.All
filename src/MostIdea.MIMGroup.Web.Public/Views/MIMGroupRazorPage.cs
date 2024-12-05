using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace MostIdea.MIMGroup.Web.Public.Views
{
    public abstract class MIMGroupRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected MIMGroupRazorPage()
        {
            LocalizationSourceName = MIMGroupConsts.LocalizationSourceName;
        }
    }
}
