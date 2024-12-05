using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.B2B
{
    public interface IDynamicEnumsAppService : IApplicationService
    {
        Task<PagedResultDto<GetDynamicEnumForViewDto>> GetAll(GetAllDynamicEnumsInput input);

        Task<GetDynamicEnumForEditOutput> GetDynamicEnumForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditDynamicEnumDto input);

        Task Delete(EntityDto<Guid> input);

        List<SelectionDto> GetEnumFiles();

    }
}