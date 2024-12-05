using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using MostIdea.MIMGroup.Configure;
using MostIdea.MIMGroup.Startup;
using MostIdea.MIMGroup.Test.Base;

namespace MostIdea.MIMGroup.GraphQL.Tests
{
    [DependsOn(
        typeof(MIMGroupGraphQLModule),
        typeof(MIMGroupTestBaseModule))]
    public class MIMGroupGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MIMGroupGraphQLTestModule).GetAssembly());
        }
    }
}