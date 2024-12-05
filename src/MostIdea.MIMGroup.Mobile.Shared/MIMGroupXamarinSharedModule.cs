using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace MostIdea.MIMGroup
{
    [DependsOn(typeof(MIMGroupClientModule), typeof(AbpAutoMapperModule))]
    public class MIMGroupXamarinSharedModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MIMGroupXamarinSharedModule).GetAssembly());
        }
    }
}