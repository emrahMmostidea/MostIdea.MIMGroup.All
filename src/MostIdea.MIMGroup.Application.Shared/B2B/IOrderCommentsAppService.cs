using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.B2B
{
    public interface IOrderCommentsAppService : IApplicationService
    {
        Task<PagedResultDto<GetOrderCommentForViewDto>> GetAll(GetAllOrderCommentsInput input);

        Task<GetOrderCommentForViewDto> GetOrderCommentForView(Guid id);

        Task<GetOrderCommentForEditOutput> GetOrderCommentForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditOrderCommentDto input);

        Task Delete(EntityDto<Guid> input);

        Task<PagedResultDto<OrderCommentOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input);

    }
}