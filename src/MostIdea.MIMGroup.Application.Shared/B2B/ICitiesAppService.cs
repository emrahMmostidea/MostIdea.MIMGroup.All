using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using System.Collections.Generic;

namespace MostIdea.MIMGroup.B2B
{
    public interface ICitiesAppService : IApplicationService
    {
        Task<PagedResultDto<GetCityForViewDto>> GetAll(GetAllCitiesInput input);

        Task<GetCityForEditOutput> GetCityForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditCityDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetCitiesToExcel(GetAllCitiesForExcelInput input);

        Task<List<CityCountryLookupTableDto>> GetAllCountryForTableDropdown();

    }
}