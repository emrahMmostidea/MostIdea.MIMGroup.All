using Abp;

namespace MostIdea.MIMGroup
{
    /// <summary>
    /// This class can be used as a base class for services in this application.
    /// It has some useful objects property-injected and has some basic methods most of services may need to.
    /// It's suitable for non domain nor application service classes.
    /// For domain services inherit <see cref="MIMGroupDomainServiceBase"/>.
    /// For application services inherit MIMGroupAppServiceBase.
    /// </summary>
    public abstract class MIMGroupServiceBase : AbpServiceBase
    {
        protected MIMGroupServiceBase()
        {
            LocalizationSourceName = MIMGroupConsts.LocalizationSourceName;
        }
    }
}