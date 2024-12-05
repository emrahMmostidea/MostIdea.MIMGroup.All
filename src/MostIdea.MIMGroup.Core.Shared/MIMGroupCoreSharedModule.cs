using Abp.Modules;
using Abp.Reflection.Extensions;

namespace MostIdea.MIMGroup
{
    public class MIMGroupCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MIMGroupCoreSharedModule).GetAssembly());
        }
    }
}