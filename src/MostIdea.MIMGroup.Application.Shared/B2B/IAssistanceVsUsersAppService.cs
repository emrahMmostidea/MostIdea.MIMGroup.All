using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using System.Collections.Generic;

namespace MostIdea.MIMGroup.B2B
{
    public interface IAssistanceVsUsersAppService : IApplicationService
    {
        Task<PagedResultDto<GetAssistanceVsUserForViewDto>> GetAll(GetAllAssistanceVsUsersInput input);

        Task<GetAssistanceVsUserForEditOutput> GetAssistanceVsUserForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditAssistanceVsUserDto input);

        Task Delete(EntityDto<Guid> input);

        Task<PagedResultDto<AssistanceVsUserUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

        Task<List<AssistanceVsUserUserLookupTableDto>> GetAllUserForTableDropdown();

    }
}