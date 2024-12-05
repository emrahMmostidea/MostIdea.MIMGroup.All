using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.B2B.Exporting;
using MostIdea.MIMGroup.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace MostIdea.MIMGroup.B2B
{
    //[AbpAuthorizeAppPermissions.Pages_OrderItems)]
    public class OrderItemsAppService : MIMGroupAppServiceBase, IOrderItemsAppService
    {
        private readonly IRepository<OrderItem, Guid> _orderItemRepository;
        private readonly IOrderItemsExcelExporter _orderItemsExcelExporter;
        private readonly IRepository<Product, Guid> _lookup_productRepository;
        private readonly IRepository<Order, Guid> _lookup_orderRepository;
        private readonly IOrdersAppService _orderAppService;

        public OrderItemsAppService(IRepository<OrderItem, Guid> orderItemRepository, IOrderItemsExcelExporter orderItemsExcelExporter, IRepository<Product, Guid> lookupProductRepository, IRepository<Order, Guid> lookupOrderRepository, IOrdersAppService orderAppService)
        {
            _orderItemRepository = orderItemRepository;
            _orderItemsExcelExporter = orderItemsExcelExporter;
            _lookup_productRepository = lookupProductRepository;
            _lookup_orderRepository = lookupOrderRepository;
            _orderAppService = orderAppService;
        }

        public async Task<PagedResultDto<GetOrderItemForViewDto>> GetAll(GetAllOrderItemsInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                        ? (OrderItemStatusEnum)input.StatusFilter
                        : default;

            var filteredOrderItems = _orderItemRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ProductFk.TaxRateFk)
                        .Include(e => e.OrderFk)
                        .WhereIf(input.OrderId.HasValue, x => x.OrderId == input.OrderId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderOrderNoFilter), e => e.OrderFk != null && e.OrderFk.OrderNo == input.OrderOrderNoFilter);

            var pagedAndFilteredOrderItems = filteredOrderItems
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orderItems = from o in pagedAndFilteredOrderItems
                             join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                             from s1 in j1.DefaultIfEmpty()

                             join o2 in _lookup_orderRepository.GetAll() on o.OrderId equals o2.Id into j2
                             from s2 in j2.DefaultIfEmpty()

                             select new
                             {
                                 o.Price,
                                 o.Amount,
                                 o.Status,
                                 Id = o.Id,
                                 ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                 OrderOrderNo = s2 == null || s2.OrderNo == null ? "" : s2.OrderNo.ToString(),
                                 ProductImage = s1 == null || s1.Image == null ? "/Common/Images/default-picture.png" : s2.OrderNo.ToString(),
                                 TaxRate = s1 == null || s1.TaxRateFk == null ? 0 : s1.TaxRateFk.Rate,
                                 TaxName = s1 == null || s1.TaxRateFk == null ? "" : s1.TaxRateFk.Name,
                                 OrderId = o.OrderId,
                                 ProductId = o.ProductId,
                                 Product = o.ProductFk,
                                 ProductCategoryName = o.ProductFk.ProductCategoryFk.Name,
                                 ProductBrandName = o.ProductFk.ProductCategoryFk.BrandFk.Name,
                             };

            var totalCount = await filteredOrderItems.CountAsync();

            var dbList = await orderItems.ToListAsync();
            var results = new List<GetOrderItemForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderItemForViewDto()
                {
                    OrderItem = new OrderItemDto
                    {
                        Price = o.Price,
                        Amount = o.Amount,
                        Status = o.Status,
                        Id = o.Id,
                        OrderId = o.OrderId,
                        ProductId = o.ProductId,
                        Product = ObjectMapper.Map<ProductDto>(o.Product),
                    },
                    ProductName = o.ProductName,
                    ProductImage = o.ProductImage,
                    OrderOrderNo = o.OrderOrderNo,
                    TaxRate = o.TaxRate,
                    TaxName = o.TaxName,
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderItemForViewDto>(
                totalCount,
                results
            );
        }

        //[AbpAuthorizeAppPermissions.Pages_OrderItems_Edit)]
        public async Task<GetOrderItemForEditOutput> GetOrderItemForEdit(EntityDto<Guid> input)
        {
            var orderItem = await _orderItemRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderItemForEditOutput { OrderItem = ObjectMapper.Map<CreateOrEditOrderItemDto>(orderItem) };

            if (output.OrderItem.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((Guid)output.OrderItem.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.OrderItem.OrderId != null)
            {
                var _lookupOrder = await _lookup_orderRepository.FirstOrDefaultAsync((Guid)output.OrderItem.OrderId);
                output.OrderOrderNo = _lookupOrder?.OrderNo?.ToString();
            }

            return output;
        }

        public async Task<OrderItemDto> CreateOrEdit(CreateOrEditOrderItemDto input)
        {
            if (input.OrderId.HasValue)
            {
                var orderItemFromOrder = await
                    _orderItemRepository.FirstOrDefaultAsync(x =>
                        x.OrderId == input.OrderId && x.ProductId == input.ProductId);
                if (orderItemFromOrder != null)
                {
                    orderItemFromOrder.Amount++;
                    await _orderItemRepository.UpdateAsync(orderItemFromOrder);

                    return new OrderItemDto() { Amount = input.Amount, OrderId = input.OrderId.GetValueOrDefault(), Price = input.Price, Status = input.Status };
                }
            }

            if (input.Id == null)
            {
                input.Status = OrderItemStatusEnum.Added;
                return await Create(input);
            }
            else
            {
                return await Update(input);
            }
        }

        //[AbpAuthorizeAppPermissions.Pages_OrderItems_Create)]
        protected virtual async Task<OrderItemDto> Create(CreateOrEditOrderItemDto input)
        {
            if (!input.OrderId.HasValue)
            {
                var order = await _orderAppService.CreateOrEdit(new CreateOrEditOrderDto()
                {
                    OrderItems = new List<CreateOrEditOrderItemDto>() { input },
                    DeliveryAddressId = input.DeliveryId,
                    InvoiceAddressId = input.InvoiceId
                });
                input.OrderId = order.Id;

                return new OrderItemDto() { Amount = input.Amount, OrderId = order.Id, Price = input.Price, Status = input.Status };
            }
            else
            {
                var orderItem = ObjectMapper.Map<OrderItem>(input);

                await _orderItemRepository.InsertAsync(orderItem);

                return ObjectMapper.Map(orderItem, new OrderItemDto());
            }
        }

        //[AbpAuthorizeAppPermissions.Pages_OrderItems_Edit)]
        protected virtual async Task<OrderItemDto> Update(CreateOrEditOrderItemDto input)
        {
            var orderItem = await _orderItemRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, orderItem);
            return ObjectMapper.Map(orderItem, new OrderItemDto());
        }

        //[AbpAuthorizeAppPermissions.Pages_OrderItems_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _orderItemRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOrderItemsToExcel(GetAllOrderItemsForExcelInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                        ? (OrderItemStatusEnum)input.StatusFilter
                        : default;

            var filteredOrderItems = _orderItemRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.OrderFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinAmountFilter != null, e => e.Amount >= input.MinAmountFilter)
                        .WhereIf(input.MaxAmountFilter != null, e => e.Amount <= input.MaxAmountFilter)
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderOrderNoFilter), e => e.OrderFk != null && e.OrderFk.OrderNo == input.OrderOrderNoFilter);

            var query = (from o in filteredOrderItems
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_orderRepository.GetAll() on o.OrderId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetOrderItemForViewDto()
                         {
                             OrderItem = new OrderItemDto
                             {
                                 Price = o.Price,
                                 Amount = o.Amount,
                                 Status = o.Status,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             OrderOrderNo = s2 == null || s2.OrderNo == null ? "" : s2.OrderNo.ToString()
                         });

            var orderItemListDtos = await query.ToListAsync();

            return _orderItemsExcelExporter.ExportToFile(orderItemListDtos);
        }

        //[AbpAuthorizeAppPermissions.Pages_OrderItems)]
        public async Task<List<OrderItemProductLookupTableDto>> GetAllProductForTableDropdown()
        {
            return await _lookup_productRepository.GetAll()
                .Select(product => new OrderItemProductLookupTableDto
                {
                    Id = product.Id.ToString(),
                    DisplayName = product == null || product.Name == null ? "" : product.Name.ToString()
                }).ToListAsync();
        }

        //[AbpAuthorizeAppPermissions.Pages_OrderItems)]
        public async Task<List<OrderItemOrderLookupTableDto>> GetAllOrderForTableDropdown()
        {
            return await _lookup_orderRepository.GetAll()
                .Select(order => new OrderItemOrderLookupTableDto
                {
                    Id = order.Id.ToString(),
                    DisplayName = order == null || order.OrderNo == null ? "" : order.OrderNo.ToString()
                }).ToListAsync();
        }
    }
}