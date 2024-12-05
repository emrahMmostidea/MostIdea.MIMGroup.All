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
    //[AbpAuthorize(AppPermissions.Pages_Products)]
    public class ProductsAppService : MIMGroupAppServiceBase, IProductsAppService
    {
        private readonly IRepository<Product, Guid> _productRepository;
        private readonly IProductsExcelExporter _productsExcelExporter;
        private readonly IRepository<ProductCategory, Guid> _lookup_productCategoryRepository;
        private readonly IRepository<TaxRate, Guid> _lookup_taxRateRepository;
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<OrderItem, Guid> _orderItemRepository;
        private readonly IRepository<HospitalVsUser, Guid> _hospitalVsUsersRepository;

        public ProductsAppService(IRepository<Product, Guid> productRepository, IProductsExcelExporter productsExcelExporter, IRepository<ProductCategory, Guid> lookup_productCategoryRepository, IRepository<TaxRate, Guid> lookup_taxRateRepository, IRepository<Order, Guid> orderRepository, IRepository<OrderItem, Guid> orderItemRepository, IRepository<HospitalVsUser, Guid> hospitalVsUsersRepository)
        {
            _productRepository = productRepository;
            _productsExcelExporter = productsExcelExporter;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;
            _lookup_taxRateRepository = lookup_taxRateRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _hospitalVsUsersRepository = hospitalVsUsersRepository;
        }

        public async Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input)
        {
            var filteredProducts = _productRepository.GetAll()
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.ProductCategoryFk.BrandFk)
                        .Include(e => e.TaxRateFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(input.CategoryId.HasValue, e => e.ProductCategoryId == input.CategoryId)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxRateNameFilter), e => e.TaxRateFk != null && e.TaxRateFk.Name == input.TaxRateNameFilter);

            var pagedAndFilteredProducts = filteredProducts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var products = from o in pagedAndFilteredProducts
                           join o1 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join o2 in _lookup_taxRateRepository.GetAll() on o.TaxRateId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           select new
                           {
                               o.Name,
                               o.Description,
                               o.Price,
                               o.Quantity,
                               Id = o.Id,
                               ProductCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                               TaxRateName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                               TaxRate = s2 == null || s2.Name == null ? 0 : s2.Rate,
                               Category = o.ProductCategoryFk,
                               Brand = o.ProductCategoryFk.BrandFk
                           };

            var totalCount = await filteredProducts.CountAsync();

            var dbList = await products.ToListAsync();
            var results = new List<GetProductForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductForViewDto()
                {
                    Product = new ProductDto
                    {
                        Name = o.Name,
                        Description = o.Description,
                        Price = o.Price,
                        Quantity = o.Quantity,
                        Id = o.Id,
                        TaxRate = o.TaxRate,
                    },
                    ProductCategoryName = o.ProductCategoryName,
                    TaxRateName = o.TaxRateName,
                    Category = ObjectMapper.Map(o.Category, new ProductCategoryDto()),
                    Brand = ObjectMapper.Map(o.Brand, new BrandDto())
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductForViewDto>(
                totalCount,
                results
            );
        }
        
        public async Task<PagedResultDto<GetProductForViewDto>> GetAllConsignmentProducts(Guid? HospitalId, Guid? CategoryId)
        {
            //if (!HospitalId.HasValue)
            //{
            //    var hospitalVsUsers = await _hospitalVsUsersRepository.FirstOrDefaultAsync(x => x.UserId == AbpSession.UserId.Value);
            //    HospitalId = hospitalVsUsers.HospitalId;
            //}

            var result = new List<GetProductForViewDto>();
            
            var orders = await _orderRepository.GetAllIncluding()
                .Where(x => x.OrderType == OrderTypeEnum.ToConsignment && x.HospitalId == HospitalId)
                .ToListAsync();


            var products = new List<Product>();
            foreach (var order in orders)
            {
                var orderItems = _orderItemRepository
                    .GetAll()
                    .Include(x => x.ProductFk)
                    .Include(x => x.ProductFk.TaxRateFk)
                    .Include(x => x.ProductFk.ProductCategoryFk)
                    .Include(x => x.ProductFk.ProductCategoryFk.BrandFk)
                    .Where(x => x.OrderId == order.Id)
                    .WhereIf(CategoryId.HasValue,x => x.ProductFk.ProductCategoryFk.Id == CategoryId)
                    .ToList();

                foreach (var orderItem in orderItems)
                {
                    var product = products.FirstOrDefault(x => x.Id == orderItem.ProductId);
                    if (product != null)
                    {
                        product.Quantity += orderItem.Amount;
                    }
                    else
                    {
                        orderItem.ProductFk.Quantity = orderItem.Amount;
                        products.Add(orderItem.ProductFk);
                    }
                }
            }
            
            products.ForEach(x =>
            {
                var res = new GetProductForViewDto()
                {
                    Product = new ProductDto
                    {
                        Name = x.Name,
                        Description = x.Description,
                        Price = x.Price,
                        Quantity = x.Quantity,
                        Id = x.Id,
                        TaxRate = x.TaxRateFk.Rate,
                        ProductCategoryId = x.ProductCategoryId,
                        ImageId = x.ImageId
                    },
                    ProductCategoryName = x.ProductCategoryFk.Name,
                    TaxRateName = x.TaxRateFk.Name,
                    Category = ObjectMapper.Map(x.ProductCategoryFk, new ProductCategoryDto()),
                    Brand = ObjectMapper.Map(x.ProductCategoryFk.BrandFk, new BrandDto())
                };

                result.Add(res);
            });

            return new PagedResultDto<GetProductForViewDto>(
                result.Count,
                result
            );
        }

        public async Task<GetProductForViewDto> GetProductForView(Guid id)
        {
            var product = await _productRepository.GetAsync(id);

            var output = new GetProductForViewDto { Product = ObjectMapper.Map<ProductDto>(product) };

            if (output.Product.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((Guid)output.Product.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.Product.TaxRateId != null)
            {
                var _lookupTaxRate = await _lookup_taxRateRepository.FirstOrDefaultAsync((Guid)output.Product.TaxRateId);
                output.TaxRateName = _lookupTaxRate?.Name?.ToString();
            }

            return output;
        }

        //[AbpAuthorize(AppPermissions.Pages_Products_Edit)]
        public async Task<GetProductForEditOutput> GetProductForEdit(EntityDto<Guid> input)
        {
            var product = await _productRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductForEditOutput { Product = ObjectMapper.Map<CreateOrEditProductDto>(product) };

            if (output.Product.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((Guid)output.Product.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.Product.TaxRateId != null)
            {
                var _lookupTaxRate = await _lookup_taxRateRepository.FirstOrDefaultAsync((Guid)output.Product.TaxRateId);
                output.TaxRateName = _lookupTaxRate?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductDto input)
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

        //[AbpAuthorize(AppPermissions.Pages_Products_Create)]
        protected virtual async Task Create(CreateOrEditProductDto input)
        {
            var product = ObjectMapper.Map<Product>(input);

            await _productRepository.InsertAsync(product);
        }

        //[AbpAuthorize(AppPermissions.Pages_Products_Edit)]
        protected virtual async Task Update(CreateOrEditProductDto input)
        {
            var product = await _productRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, product);
        }

        //[AbpAuthorize(AppPermissions.Pages_Products_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _productRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input)
        {
            var filteredProducts = _productRepository.GetAll()
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.TaxRateFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(input.MinQuantityFilter != null, e => e.Quantity >= input.MinQuantityFilter)
                        .WhereIf(input.MaxQuantityFilter != null, e => e.Quantity <= input.MaxQuantityFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TaxRateNameFilter), e => e.TaxRateFk != null && e.TaxRateFk.Name == input.TaxRateNameFilter);

            var query = (from o in filteredProducts
                         join o1 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_taxRateRepository.GetAll() on o.TaxRateId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProductForViewDto()
                         {
                             Product = new ProductDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Price = o.Price,
                                 Quantity = o.Quantity,
                                 Id = o.Id
                             },
                             ProductCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             TaxRateName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var productListDtos = await query.ToListAsync();

            return _productsExcelExporter.ExportToFile(productListDtos);
        }

        //[AbpAuthorize(AppPermissions.Pages_Products)]
        public async Task<List<ProductProductCategoryLookupTableDto>> GetAllProductCategoryForTableDropdown()
        {
            return await _lookup_productCategoryRepository.GetAll()
                .Select(productCategory => new ProductProductCategoryLookupTableDto
                {
                    Id = productCategory.Id.ToString(),
                    DisplayName = productCategory == null || productCategory.Name == null ? "" : productCategory.Name.ToString()
                }).ToListAsync();
        }

        //[AbpAuthorize(AppPermissions.Pages_Products)]
        public async Task<List<ProductTaxRateLookupTableDto>> GetAllTaxRateForTableDropdown()
        {
            return await _lookup_taxRateRepository.GetAll()
                .Select(taxRate => new ProductTaxRateLookupTableDto
                {
                    Id = taxRate.Id.ToString(),
                    DisplayName = taxRate == null || taxRate.Name == null ? "" : taxRate.Name.ToString()
                }).ToListAsync();
        }
         
    }
}