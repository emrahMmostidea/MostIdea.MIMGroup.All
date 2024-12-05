using Abp.Dependency;
using MostIdea.MIMGroup.Configuration;
using MostIdea.MIMGroup.Url;
using MostIdea.MIMGroup.Web.Url;

namespace MostIdea.MIMGroup.Web.Public.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor appConfigurationAccessor) :
            base(appConfigurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:AdminWebSiteRootAddress";
    }
}