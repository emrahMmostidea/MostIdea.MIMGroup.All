using Abp.Modules;
using Abp.Reflection.Extensions;

namespace MostIdea.MIMGroup
{
    [DependsOn(typeof(MIMGroupXamarinSharedModule))]
    public class MIMGroupXamarinIosModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MIMGroupXamarinIosModule).GetAssembly());
        }
    }
}