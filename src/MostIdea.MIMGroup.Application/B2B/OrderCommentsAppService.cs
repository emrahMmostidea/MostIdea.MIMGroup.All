using MostIdea.MIMGroup.B2B;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using MostIdea.MIMGroup.Storage;

namespace MostIdea.MIMGroup.B2B
{
    [AbpAuthorize(AppPermissions.Pages_OrderComments)]
    public class OrderCommentsAppService : MIMGroupAppServiceBase, IOrderCommentsAppService
    {
        private readonly IRepository<OrderComment, Guid> _orderCommentRepository;
        private readonly IRepository<Order, Guid> _lookup_orderRepository;

        public OrderCommentsAppService(IRepository<OrderComment, Guid> orderCommentRepository, IRepository<Order, Guid> lookup_orderRepository)
        {
            _orderCommentRepository = orderCommentRepository;
            _lookup_orderRepository = lookup_orderRepository;

        }

        public async Task<PagedResultDto<GetOrderCommentForViewDto>> GetAll(GetAllOrderCommentsInput input)
        {

            var filteredOrderComments = _orderCommentRepository.GetAll()
                        .Include(e => e.OrderFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Comment.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CommentFilter), e => e.Comment == input.CommentFilter)
                        .WhereIf(input.OrderId.HasValue, x => x.OrderId == input.OrderId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderOrderNoFilter), e => e.OrderFk != null && e.OrderFk.OrderNo == input.OrderOrderNoFilter);

            var pagedAndFilteredOrderComments = filteredOrderComments
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orderComments = from o in pagedAndFilteredOrderComments
                                join o1 in _lookup_orderRepository.GetAll() on o.OrderId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                select new
                                {

                                    o.Comment,
                                    Id = o.Id,
                                    OrderOrderNo = s1 == null || s1.OrderNo == null ? "" : s1.OrderNo.ToString(),
                                    CreationTime = o.CreationTime
                                };

            var totalCount = await filteredOrderComments.CountAsync();

            var dbList = await orderComments.ToListAsync();
            var results = new List<GetOrderCommentForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderCommentForViewDto()
                {
                    OrderComment = new OrderCommentDto
                    {

                        Comment = o.Comment,
                        Id = o.Id,
                        CreationTime = o.CreationTime
                    },
                    OrderOrderNo = o.OrderOrderNo
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderCommentForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOrderCommentForViewDto> GetOrderCommentForView(Guid id)
        {
            var orderComment = await _orderCommentRepository.GetAsync(id);

            var output = new GetOrderCommentForViewDto { OrderComment = ObjectMapper.Map<OrderCommentDto>(orderComment) };

            if (output.OrderComment.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((Guid)output.OrderComment.OrderId);
                output.OrderOrderNo = _lookupOrder?.OrderNo?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OrderComments_Edit)]
        public async Task<GetOrderCommentForEditOutput> GetOrderCommentForEdit(EntityDto<Guid> input)
        {
            var orderComment = await _orderCommentRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderCommentForEditOutput { OrderComment = ObjectMapper.Map<CreateOrEditOrderCommentDto>(orderComment) };

            if (output.OrderComment.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((Guid)output.OrderComment.OrderId);
                output.OrderOrderNo = _lookupOrder?.OrderNo?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOrderCommentDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_OrderComments_Create)]
        protected virtual async Task Create(CreateOrEditOrderCommentDto input)
        {
            var orderComment = ObjectMapper.Map<OrderComment>(input);

            await _orderCommentRepository.InsertAsync(orderComment);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderComments_Edit)]
        protected virtual async Task Update(CreateOrEditOrderCommentDto input)
        {
            var orderComment = await _orderCommentRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, orderComment);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderComments_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _orderCommentRepository.DeleteAsync(input.Id);
        }

        [AbpAuthorize(AppPermissions.Pages_OrderComments)]
        public async Task<PagedResultDto<OrderCommentOrderLookupTableDto>> GetAllOrderForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_orderRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.OrderNo != null && e.OrderNo.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var orderList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderCommentOrderLookupTableDto>();
            foreach (var order in orderList)
            {
                lookupTableDtoList.Add(new OrderCommentOrderLookupTableDto
                {
                    Id = order.Id.ToString(),
                    DisplayName = order.OrderNo?.ToString()
                });
            }

            return new PagedResultDto<OrderCommentOrderLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}