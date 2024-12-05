using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using System.Collections.Generic;

namespace MostIdea.MIMGroup.B2B
{
    public interface IWarehousesAppService : IApplicationService
    {
        Task<PagedResultDto<GetWarehouseForViewDto>> GetAll(GetAllWarehousesInput input);

        Task<GetWarehouseForEditOutput> GetWarehouseForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditWarehouseDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetWarehousesToExcel(GetAllWarehousesForExcelInput input);

        Task<List<WarehouseDistrictLookupTableDto>> GetAllDistrictForTableDropdown();

    }
}