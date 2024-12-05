using System.Threading.Tasks;
using Abp.Application.Services;
using MostIdea.MIMGroup.Editions.Dto;
using MostIdea.MIMGroup.MultiTenancy.Dto;

namespace MostIdea.MIMGroup.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}