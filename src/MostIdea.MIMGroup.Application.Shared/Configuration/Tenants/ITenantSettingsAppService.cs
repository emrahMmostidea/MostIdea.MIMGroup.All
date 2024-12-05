using System.Threading.Tasks;
using Abp.Application.Services;
using MostIdea.MIMGroup.Configuration.Tenants.Dto;

namespace MostIdea.MIMGroup.Configuration.Tenants
{
    public interface ITenantSettingsAppService : IApplicationService
    {
        Task<TenantSettingsEditDto> GetAllSettings();

        Task UpdateAllSettings(TenantSettingsEditDto input);

        Task ClearLogo();

        Task ClearCustomCss();
    }
}
