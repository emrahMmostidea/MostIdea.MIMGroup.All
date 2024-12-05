using Microsoft.Extensions.Configuration;

namespace MostIdea.MIMGroup.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
