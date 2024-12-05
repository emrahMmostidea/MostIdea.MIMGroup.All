using Abp.Modules;
using Abp.Reflection.Extensions;

namespace MostIdea.MIMGroup
{
    public class MIMGroupClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MIMGroupClientModule).GetAssembly());
        }
    }
}
