using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.B2B
{
    public interface ICountriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCountryForViewDto>> GetAll(GetAllCountriesInput input);

        Task<GetCountryForEditOutput> GetCountryForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditCountryDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetCountriesToExcel(GetAllCountriesForExcelInput input);

    }
}