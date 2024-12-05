using System.Threading.Tasks;
using Abp.Application.Services;
using MostIdea.MIMGroup.Configuration.Host.Dto;

namespace MostIdea.MIMGroup.Configuration.Host
{
    public interface IHostSettingsAppService : IApplicationService
    {
        Task<HostSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(HostSettingsEditDto input);

        Task SendTestEmail(SendTestEmailInput input);
    }
}
