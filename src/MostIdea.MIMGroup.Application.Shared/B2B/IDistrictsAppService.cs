using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using System.Collections.Generic;

namespace MostIdea.MIMGroup.B2B
{
    public interface IDistrictsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDistrictForViewDto>> GetAll(GetAllDistrictsInput input);

        Task<GetDistrictForEditOutput> GetDistrictForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditDistrictDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetDistrictsToExcel(GetAllDistrictsForExcelInput input);

        Task<List<DistrictCityLookupTableDto>> GetAllCityForTableDropdown();

    }
}