using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using MostIdea.MIMGroup.Authorization;
using MostIdea.MIMGroup.Authorization.Users;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.B2B.Exporting;
using MostIdea.MIMGroup.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using MostIdea.MIMGroup.Authorization.Roles;
using MostIdea.MIMGroup.Authorization.Roles.Dto;

namespace MostIdea.MIMGroup.B2B
{
    //[AbpAuthorize(AppPermissions.Pages_Orders)]
    public class OrdersAppService : MIMGroupAppServiceBase, IOrdersAppService
    {
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IOrdersExcelExporter _ordersExcelExporter;
        private readonly IRepository<AddressInformation, Guid> _lookup_addressInformationRepository;
        private readonly IRepository<User, long> _lookup_userRepository;
        private readonly IRepository<Hospital, Guid> _lookup_hospitalRepository;
        private readonly IRepository<Warehouse, Guid> _lookup_warehouseRepository;
        private readonly IRepository<HospitalVsUser, Guid> _hospitalVsUserRepository;
        private readonly IRepository<OrderItem, Guid> _orderItemsRepository;
        private readonly IRepository<OrderComment, Guid> _orderCommentRepository;
        private readonly IHospitalsAppService _hospitalAppService;
        private readonly IRoleAppService _roleAppService;

        public OrdersAppService(IRepository<Order, Guid> orderRepository, IOrdersExcelExporter ordersExcelExporter, IRepository<AddressInformation, Guid> lookupAddressInformationRepository, IRepository<User, long> lookupUserRepository, IRepository<Hospital, Guid> lookupHospitalRepository, IRepository<Warehouse, Guid> lookupWarehouseRepository, IRepository<HospitalVsUser, Guid> hospitalVsUserRepository, IRepository<OrderItem, Guid> orderItemsRepository, IRepository<OrderComment, Guid> orderCommentRepository, IHospitalsAppService hospitalAppService, IRoleAppService roleAppService)
        {
            _orderRepository = orderRepository;
            _ordersExcelExporter = ordersExcelExporter;
            _lookup_addressInformationRepository = lookupAddressInformationRepository;
            _lookup_userRepository = lookupUserRepository;
            _lookup_hospitalRepository = lookupHospitalRepository;
            _lookup_warehouseRepository = lookupWarehouseRepository;
            _hospitalVsUserRepository = hospitalVsUserRepository;
            _orderItemsRepository = orderItemsRepository;
            _orderCommentRepository = orderCommentRepository;
            _hospitalAppService = hospitalAppService;
            _roleAppService = roleAppService;
        }
        public async Task<PagedResultDto<GetOrderForViewDto>> GetAll(GetAllOrdersInput input)
        {


            var statusFilter = input.StatusFilter.HasValue
                        ? (OrderStatusEnum)input.StatusFilter
                        : default;

            var filteredOrders = _orderRepository.GetAll()
                        .Include(e => e.DeliveryAddressFk)
                        .Include(e => e.InvoiceAddressFk)
                        .Include(e => e.CourierFk)
                        .Include(e => e.HospitalFk)
                        .Include(e => e.DoctorFk)
                        .Include(e => e.WarehouseFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.OrderNo.Contains(input.Filter))
                        .WhereIf(input.MinTotalFilter != null, e => e.Total >= input.MinTotalFilter)
                        .WhereIf(input.MaxTotalFilter != null, e => e.Total <= input.MaxTotalFilter)
                        .WhereIf(input.MinTaxFilter != null, e => e.Tax >= input.MinTaxFilter)
                        .WhereIf(input.MaxTaxFilter != null, e => e.Tax <= input.MaxTaxFilter)
                        .WhereIf(input.MinGrandTotalFilter != null, e => e.GrandTotal >= input.MinGrandTotalFilter)
                        .WhereIf(input.MaxGrandTotalFilter != null, e => e.GrandTotal <= input.MaxGrandTotalFilter)
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderNoFilter), e => e.OrderNo == input.OrderNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressInformationNameFilter), e => e.InvoiceAddressFk != null && e.InvoiceAddressFk.Name == input.AddressInformationNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.CourierFk != null && e.CourierFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HospitalNameFilter), e => e.HospitalFk != null && e.HospitalFk.Name == input.HospitalNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.DoctorFk != null && e.DoctorFk.Name == input.UserName2Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WarehouseNameFilter), e => e.WarehouseFk != null && e.WarehouseFk.Name == input.WarehouseNameFilter);


            var user = await _lookup_userRepository.FirstOrDefaultAsync(x => x.Id == AbpSession.UserId.Value); 
            var selectedHospital = await _hospitalAppService.GetSelectedHospital();
            if (!await UserManager.IsInRoleAsync(user, "Admin"))
            {
                if (user.UserType == UserTypeEnum.Doctor || user.UserType == UserTypeEnum.Dealer)
                {
                    filteredOrders = filteredOrders.Where(e => e.DoctorFk.Id == user.Id);
                }
                else if (user.UserType == UserTypeEnum.HospitalManager)
                {
                    if (selectedHospital != null)
                    {
                        var hospitalUsers = _hospitalVsUserRepository.GetAll().Where(x => x.HospitalId == selectedHospital.Id).Select(x => x.UserId).ToList();
                        filteredOrders = filteredOrders.Where(e => hospitalUsers.Contains(e.DoctorId.Value));
                    }
                }

                //TODO: Assistance ve Satış Temsilcisi rollerini ekle
            }
            else
            {
                if (selectedHospital != null)
                {
                    filteredOrders = filteredOrders.Where(e => e.HospitalId.Value == selectedHospital.Id);
                }
            }
            

            var pagedAndFilteredOrders = filteredOrders
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orders = from o in pagedAndFilteredOrders
                         join o1 in _lookup_addressInformationRepository.GetAll() on o.InvoiceAddressId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_userRepository.GetAll() on o.CourierId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_hospitalRepository.GetAll() on o.HospitalId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_userRepository.GetAll() on o.DoctorId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_warehouseRepository.GetAll() on o.WarehouseId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         select new
                         {
                             o.Total,
                             o.Tax,
                             o.GrandTotal,
                             o.Status,
                             o.OrderNo,
                             Id = o.Id,
                             AddressInformationName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             HospitalName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             UserName2 = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                             WarehouseName = s5 == null || s5.Name == null ? "" : s5.Name.ToString(),
                             CreationTime = o.CreationTime,
                             PatientName = o.PatientName,
                             PatientSurname = o.PatientSurname,
                             OperationTime = o.OperationTime,
                             PaymentType = o.PaymentType,
                             Doctor = o.DoctorFk.FullName,
                             o.DoctorId,
                             o.OrderType
                         };

            var totalCount = await filteredOrders.CountAsync();

            var dbList = await orders.ToListAsync();
            var results = new List<GetOrderForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderForViewDto()
                {
                    Order = new OrderDto
                    {
                        Total = o.Total,
                        Tax = o.Tax,
                        GrandTotal = o.GrandTotal,
                        Status = o.Status,
                        OrderNo = o.OrderNo,
                        Id = o.Id,
                        PatientName = o.PatientName,
                        PatientSurname = o.PatientSurname,
                        OperationTime = o.OperationTime,
                        PaymentType = o.PaymentType,
                        CreatedDate = o.CreationTime,
                        DoctorId = o.DoctorId.HasValue ? o.DoctorId.Value : AbpSession.UserId.Value,
                        OrderType = o.OrderType
                    },
                    AddressInformationName = o.AddressInformationName,
                    UserName = o.UserName,
                    HospitalName = o.HospitalName,
                    UserName2 = o.UserName2,
                    WarehouseName = o.WarehouseName,
                    Doctor = o.Doctor
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderForViewDto>(
                totalCount,
                results
            );
        }

        public async Task<bool> ShowConsignmentAction()
        {
            var hospitalVsDoctors = await _hospitalVsUserRepository.GetAll()
                .Where(x => x.UserId == AbpSession.UserId.Value)
                .ToListAsync();

            var hospitalIds = hospitalVsDoctors.Select(x => x.HospitalId).ToList();

            return _orderRepository.GetAll()
                .Any(x => x.OrderType == OrderTypeEnum.ToConsignment && hospitalIds.Contains(x.HospitalId.Value));
        }



        public async Task<List<OrderProductDto>> GetOrderProducts(Guid orderId)
        {
            return await _orderItemsRepository
                .GetAllIncluding()
                .Where(x => x.OrderId == orderId)
                .Include(x => x.ProductFk)
                .Include(x => x.ProductFk.TaxRateFk)
                .Include(x => x.ProductFk.ProductCategoryFk)
                .Include(x => x.ProductFk.ProductCategoryFk.BrandFk)
                .Include(x => x.OrderFk)
                .Select(x => new OrderProductDto
                {
                    OrderId = x.OrderId,
                    Amount = x.Amount,
                    ProductId = x.ProductId,
                    Status = x.Status,
                    OrderItemId = x.Id,
                    ProductName = x.ProductFk.Name,
                    ProductImageId = x.ProductFk.ImageId,
                    ProductCategoryId = x.ProductFk.ProductCategoryId,
                    ProductCategory = x.ProductFk.Name,
                    ProductBrandId = x.ProductFk.ProductCategoryFk.BrandId,
                    ProductBrandName = x.ProductFk.ProductCategoryFk.BrandFk.Name,
                    ProductDescription = x.ProductFk.Description,
                    ProductPrice = x.Price,
                    TaxRateName = x.ProductFk.TaxRateFk.Name,
                    TaxRate = x.ProductFk.TaxRateFk.Rate,
                    TaxRateId = x.ProductFk.TaxRateId,
                }).ToListAsync();
        }

        //[AbpAuthorize(AppPermissions.Pages_Orders_Edit)]
        public async Task<GetOrderForEditOutput> GetOrderForEdit(EntityDto<Guid> input)
        {
            var order = await _orderRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderForEditOutput { Order = ObjectMapper.Map<CreateOrEditOrderDto>(order) };
            output.Order.PatientName = order.PatientName;
            output.Order.PatientSurname = order.PatientSurname;
            output.Order.OperationTime = order.OperationTime;

            if (output.Order.DeliveryAddressId != null)
            {
                var _lookupAddressInformation = await _lookup_addressInformationRepository.FirstOrDefaultAsync((Guid)output.Order.DeliveryAddressId);
                output.AddressInformationName = _lookupAddressInformation?.Name?.ToString();
            }

            if (output.Order.CourierId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Order.CourierId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            if (output.Order.HospitalId != null)
            {
                var _lookupHospital = await _lookup_hospitalRepository.FirstOrDefaultAsync((Guid)output.Order.HospitalId);
                output.HospitalName = _lookupHospital?.Name?.ToString();
            }

            if (output.Order.DoctorId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.Order.DoctorId);
                output.UserName2 = _lookupUser?.Name?.ToString();
            }

            if (output.Order.WarehouseId != null)
            {
                var _lookupWarehouse = await _lookup_warehouseRepository.FirstOrDefaultAsync((Guid)output.Order.WarehouseId);
                output.WarehouseName = _lookupWarehouse?.Name?.ToString();
            }

            return output;
        }

        public async Task<OrderDto> CreateOrEdit(CreateOrEditOrderDto input)
        {
            if (input.Id == null || input.Id == Guid.Empty)
            {
                return await Create(input);
            }
            else
            {
                return await Update(input);
            }
        }

        //[AbpAuthorize(AppPermissions.Pages_Orders_Create)]
        protected virtual async Task<OrderDto> Create(CreateOrEditOrderDto input)
        {
            var userId = AbpSession.UserId;
            if (!userId.HasValue)
            {
                throw new UserFriendlyException("Kullanıcı bulunamadı !");
                // TODO: UserTypeEnum'a göre işlem yap
            }

            if (input.OrderNo.IsNullOrWhiteSpace())
            {
                input.OrderNo = (await _orderRepository.CountAsync() + 1).ToString();
            }

            if (!input.HospitalId.HasValue)
            {
                var address = await _lookup_addressInformationRepository.FirstOrDefaultAsync(input.InvoiceAddressId.Value);
                input.HospitalId = address.HospitalId;
            }

            var hospitalVsUser = await _hospitalVsUserRepository.FirstOrDefaultAsync(x => x.UserId == userId || x.HospitalId == input.HospitalId.Value);
            if (hospitalVsUser == null)
            {
                throw new UserFriendlyException("Kullanıcı işlemi ilgili kuruma sipariş veremez !");
            }

            var order = new Order
            {
                HospitalId = input.HospitalId.Value,
                DoctorId = userId.Value,
                DeliveryAddressId = input.DeliveryAddressId,
                InvoiceAddressId = input.InvoiceAddressId,
                OrderNo = Convert.ToInt32(input.OrderNo).ToString("D6"),
                Status = OrderStatusEnum.NewOrder,
                PatientName = input.PatientName,
                PatientSurname = input.PatientSurname,
                PaymentType = input.PaymentType,
                OperationTime = input.OperationTime,
                OrderType = input.OrderType,

            };

            await _orderRepository.InsertAsync(order);

            foreach (var item in input.OrderItems)
            {
                item.OrderId = order.Id;
                await _orderItemsRepository.InsertAsync(ObjectMapper.Map(item, new OrderItem()));
            }

            CurrentUnitOfWork.SaveChanges();

            var orderItems = await _orderItemsRepository.GetAll()
                .Include(x => x.ProductFk)
                .Include(x => x.ProductFk.TaxRateFk)
                .Where(x => x.OrderId == order.Id).ToListAsync();
            order.GrandTotal = orderItems.Sum(x => x.Amount * (x.Price * (x.ProductFk.TaxRateFk.Rate / 100) + 1));
            order.Total = orderItems.Sum(x => x.Amount * x.Price);
            order.Tax = orderItems.Sum(x => (x.Amount * x.Price) * x.ProductFk.TaxRateFk.Rate);

            await _orderRepository.UpdateAsync(order);

            if (!input.Comment.IsNullOrEmpty())
            {
                var comment = new OrderComment()
                {
                    Comment = input.Comment,
                    OrderId = order.Id,
                };
                await _orderCommentRepository.InsertAsync(comment);
            }

            return ObjectMapper.Map(order, new OrderDto());
        }

        //[AbpAuthorize(AppPermissions.Pages_Orders_Edit)]
        protected virtual async Task<OrderDto> Update(CreateOrEditOrderDto input)
        {
            var invoiceAddress = await _lookup_addressInformationRepository.FirstOrDefaultAsync(input.InvoiceAddressId.Value);

            var order = await _orderRepository.FirstOrDefaultAsync(input.Id.Value);

            order.HospitalId = invoiceAddress.HospitalId;
            order.DoctorId = input.DoctorId;
            order.DeliveryAddressId = input.DeliveryAddressId;
            order.InvoiceAddressId = input.InvoiceAddressId;
            order.OrderNo = input.OrderNo;
            order.Status = input.Status;
            order.PatientName = input.PatientName;
            order.PatientSurname = input.PatientSurname;
            order.OperationTime = input.OperationTime.Value;

            var orderItems = await _orderItemsRepository.GetAll()
                .Include(x => x.ProductFk)
                .Include(x => x.ProductFk.TaxRateFk)
                .Where(x => x.OrderId == order.Id).ToListAsync();

            order.GrandTotal = orderItems.Sum(x => x.Amount * (x.Price * (x.ProductFk.TaxRateFk.Rate / 100) + 1));
            order.Total = orderItems.Sum(x => x.Amount * x.Price);
            order.Tax = orderItems.Sum(x => (x.Amount * x.Price) * x.ProductFk.TaxRateFk.Rate);

            await _orderRepository.UpdateAsync(order);


            input.OrderItems.ForEach(async x =>
            {
                var orderItem = orderItems.FirstOrDefault(y => y.Id == x.Id);
                if (orderItem == null)
                {
                    x.OrderId = order.Id;
                    await _orderItemsRepository.InsertAsync(ObjectMapper.Map(x, new OrderItem()));
                }
                else
                {
                    orderItem.Amount = x.Amount;
                    orderItem.Price = x.Price;
                    orderItem.ProductId = x.ProductId;
                    await _orderItemsRepository.UpdateAsync(orderItem);
                }
            });

            return ObjectMapper.Map(order, new OrderDto());
        }

        //[AbpAuthorize(AppPermissions.Pages_Orders_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _orderRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOrdersToExcel(GetAllOrdersForExcelInput input)
        {
            var statusFilter = input.StatusFilter.HasValue
                        ? (OrderStatusEnum)input.StatusFilter
                        : default;

            var filteredOrders = _orderRepository.GetAll()
                        .Include(e => e.InvoiceAddressFk)
                        .Include(e => e.DeliveryAddressFk)
                        .Include(e => e.CourierFk)
                        .Include(e => e.HospitalFk)
                        .Include(e => e.DoctorFk)
                        .Include(e => e.WarehouseFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.OrderNo.Contains(input.Filter))
                        .WhereIf(input.MinTotalFilter != null, e => e.Total >= input.MinTotalFilter)
                        .WhereIf(input.MaxTotalFilter != null, e => e.Total <= input.MaxTotalFilter)
                        .WhereIf(input.MinTaxFilter != null, e => e.Tax >= input.MinTaxFilter)
                        .WhereIf(input.MaxTaxFilter != null, e => e.Tax <= input.MaxTaxFilter)
                        .WhereIf(input.MinGrandTotalFilter != null, e => e.GrandTotal >= input.MinGrandTotalFilter)
                        .WhereIf(input.MaxGrandTotalFilter != null, e => e.GrandTotal <= input.MaxGrandTotalFilter)
                        .WhereIf(input.StatusFilter.HasValue && input.StatusFilter > -1, e => e.Status == statusFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OrderNoFilter), e => e.OrderNo == input.OrderNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AddressInformationNameFilter), e => e.InvoiceAddressFk != null && e.InvoiceAddressFk.Name == input.AddressInformationNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.CourierFk != null && e.CourierFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HospitalNameFilter), e => e.HospitalFk != null && e.HospitalFk.Name == input.HospitalNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.DoctorFk != null && e.DoctorFk.Name == input.UserName2Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WarehouseNameFilter), e => e.WarehouseFk != null && e.WarehouseFk.Name == input.WarehouseNameFilter);

            var query = (from o in filteredOrders
                         join o1 in _lookup_addressInformationRepository.GetAll() on o.InvoiceAddressId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_userRepository.GetAll() on o.CourierId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_hospitalRepository.GetAll() on o.HospitalId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_userRepository.GetAll() on o.DoctorId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         join o5 in _lookup_warehouseRepository.GetAll() on o.WarehouseId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         select new GetOrderForViewDto()
                         {
                             Order = new OrderDto
                             {
                                 Total = o.Total,
                                 Tax = o.Tax,
                                 GrandTotal = o.GrandTotal,
                                 Status = o.Status,
                                 OrderNo = o.OrderNo,
                                 Id = o.Id
                             },
                             AddressInformationName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             HospitalName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             UserName2 = s4 == null || s4.Name == null ? "" : s4.Name.ToString(),
                             WarehouseName = s5 == null || s5.Name == null ? "" : s5.Name.ToString()
                         });

            var orderListDtos = await query.ToListAsync();

            return _ordersExcelExporter.ExportToFile(orderListDtos);
        }

        //[AbpAuthorize(AppPermissions.Pages_Orders)]
        public async Task<List<OrderAddressInformationLookupTableDto>> GetAllAddressInformationForTableDropdown()
        {
            return await _lookup_addressInformationRepository.GetAll()
                .Select(addressInformation => new OrderAddressInformationLookupTableDto
                {
                    Id = addressInformation.Id.ToString(),
                    DisplayName = addressInformation == null || addressInformation.Name == null ? "" : addressInformation.Name.ToString()
                }).ToListAsync();
        }

        //[AbpAuthorize(AppPermissions.Pages_Orders)]
        public async Task<List<OrderUserLookupTableDto>> GetAllUserForTableDropdown()
        {
            return await _lookup_userRepository.GetAll()
                .Select(user => new OrderUserLookupTableDto
                {
                    Id = user.Id,
                    DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
                }).ToListAsync();
        }

        //[AbpAuthorize(AppPermissions.Pages_Orders)]
        public async Task<List<OrderHospitalLookupTableDto>> GetAllHospitalForTableDropdown()
        {
            return await _lookup_hospitalRepository.GetAll()
                .Select(hospital => new OrderHospitalLookupTableDto
                {
                    Id = hospital.Id.ToString(),
                    DisplayName = hospital == null || hospital.Name == null ? "" : hospital.Name.ToString()
                }).ToListAsync();
        }

        //[AbpAuthorize(AppPermissions.Pages_Orders)]
        public async Task<List<OrderWarehouseLookupTableDto>> GetAllWarehouseForTableDropdown()
        {
            return await _lookup_warehouseRepository.GetAll()
                .Select(warehouse => new OrderWarehouseLookupTableDto
                {
                    Id = warehouse.Id.ToString(),
                    DisplayName = warehouse == null || warehouse.Name == null ? "" : warehouse.Name.ToString()
                }).ToListAsync();
        }

        //[AbpAuthorize(AppPermissions.Pages_Orders)]
        public async Task<OrderPageDto> GetOrderPage()
        {
            var output = new OrderPageDto();

            return output;
        }

    }
}