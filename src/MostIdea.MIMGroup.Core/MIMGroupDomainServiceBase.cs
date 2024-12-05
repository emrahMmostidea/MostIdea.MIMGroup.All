using Abp.Domain.Services;

namespace MostIdea.MIMGroup
{
    public abstract class MIMGroupDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected MIMGroupDomainServiceBase()
        {
            LocalizationSourceName = MIMGroupConsts.LocalizationSourceName;
        }
    }
}
