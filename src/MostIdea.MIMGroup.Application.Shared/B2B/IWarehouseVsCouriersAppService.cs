using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using System.Collections.Generic;
using System.Collections.Generic;

namespace MostIdea.MIMGroup.B2B
{
    public interface IWarehouseVsCouriersAppService : IApplicationService
    {
        Task<PagedResultDto<GetWarehouseVsCourierForViewDto>> GetAll(GetAllWarehouseVsCouriersInput input);

        Task<GetWarehouseVsCourierForEditOutput> GetWarehouseVsCourierForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditWarehouseVsCourierDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetWarehouseVsCouriersToExcel(GetAllWarehouseVsCouriersForExcelInput input);

        Task<List<WarehouseVsCourierUserLookupTableDto>> GetAllUserForTableDropdown();

        Task<List<WarehouseVsCourierWarehouseLookupTableDto>> GetAllWarehouseForTableDropdown();

    }
}