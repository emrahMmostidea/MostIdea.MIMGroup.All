using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace MostIdea.MIMGroup.Startup
{
    [DependsOn(typeof(MIMGroupCoreModule))]
    public class MIMGroupGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MIMGroupGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}