using Abp.Modules;
using Abp.Reflection.Extensions;

namespace MostIdea.MIMGroup
{
    [DependsOn(typeof(MIMGroupCoreSharedModule))]
    public class MIMGroupApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MIMGroupApplicationSharedModule).GetAssembly());
        }
    }
}