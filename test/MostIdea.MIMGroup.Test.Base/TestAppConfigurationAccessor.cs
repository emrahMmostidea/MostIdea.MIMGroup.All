using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using MostIdea.MIMGroup.Configuration;

namespace MostIdea.MIMGroup.Test.Base
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(MIMGroupTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}
