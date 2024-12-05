using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using System.Collections.Generic; 

namespace MostIdea.MIMGroup.B2B
{
    public interface IOrderItemsAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderItemForViewDto>> GetAll(GetAllOrderItemsInput input);

        Task<GetOrderItemForEditOutput> GetOrderItemForEdit(EntityDto<Guid> input);

        Task<OrderItemDto> CreateOrEdit(CreateOrEditOrderItemDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetOrderItemsToExcel(GetAllOrderItemsForExcelInput input);

        Task<List<OrderItemProductLookupTableDto>> GetAllProductForTableDropdown();

        Task<List<OrderItemOrderLookupTableDto>> GetAllOrderForTableDropdown();

    }
}