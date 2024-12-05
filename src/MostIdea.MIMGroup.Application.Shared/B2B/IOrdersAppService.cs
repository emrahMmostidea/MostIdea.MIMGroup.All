using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MostIdea.MIMGroup.B2B
{
    public interface IOrdersAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderForViewDto>> GetAll(GetAllOrdersInput input);

        Task<GetOrderForEditOutput> GetOrderForEdit(EntityDto<Guid> input);

        Task<OrderDto> CreateOrEdit(CreateOrEditOrderDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetOrdersToExcel(GetAllOrdersForExcelInput input);

        Task<List<OrderAddressInformationLookupTableDto>> GetAllAddressInformationForTableDropdown();

        Task<List<OrderUserLookupTableDto>> GetAllUserForTableDropdown();

        Task<List<OrderHospitalLookupTableDto>> GetAllHospitalForTableDropdown();

        Task<List<OrderWarehouseLookupTableDto>> GetAllWarehouseForTableDropdown();

        Task<bool> ShowConsignmentAction();

    }
}