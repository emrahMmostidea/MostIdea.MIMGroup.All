using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using System.Collections.Generic;

namespace MostIdea.MIMGroup.B2B
{
    public interface IDynamicEnumItemsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDynamicEnumItemForViewDto>> GetAll(GetAllDynamicEnumItemsInput input);

        Task<GetDynamicEnumItemForViewDto> GetDynamicEnumItemForView(Guid id);

        Task<GetDynamicEnumItemForEditOutput> GetDynamicEnumItemForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditDynamicEnumItemDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetDynamicEnumItemsToExcel(GetAllDynamicEnumItemsForExcelInput input);

        Task<List<DynamicEnumItemDynamicEnumLookupTableDto>> GetAllDynamicEnumForTableDropdown();

    }
}