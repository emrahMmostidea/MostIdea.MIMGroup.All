using Abp.Application.Services;
using MostIdea.MIMGroup.Dto;
using MostIdea.MIMGroup.Logging.Dto;

namespace MostIdea.MIMGroup.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
