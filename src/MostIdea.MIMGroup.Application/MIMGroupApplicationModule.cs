using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using MostIdea.MIMGroup.Authorization;

namespace MostIdea.MIMGroup
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(MIMGroupApplicationSharedModule),
        typeof(MIMGroupCoreModule)
        )]
    public class MIMGroupApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MIMGroupApplicationModule).GetAssembly());
        }
    }
}