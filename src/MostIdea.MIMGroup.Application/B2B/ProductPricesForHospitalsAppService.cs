using MostIdea.MIMGroup.B2B;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using MostIdea.MIMGroup.B2B.Exporting;
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
    [AbpAuthorize(AppPermissions.Pages_ProductPricesForHospitals)]
    public class ProductPricesForHospitalsAppService : MIMGroupAppServiceBase, IProductPricesForHospitalsAppService
    {
        private readonly IRepository<ProductPricesForHospital, Guid> _productPricesForHospitalRepository;
        private readonly IProductPricesForHospitalsExcelExporter _productPricesForHospitalsExcelExporter;
        private readonly IRepository<Product, Guid> _lookup_productRepository;
        private readonly IRepository<ProductCategory, Guid> _lookup_productCategoryRepository;

        public ProductPricesForHospitalsAppService(IRepository<ProductPricesForHospital, Guid> productPricesForHospitalRepository, IProductPricesForHospitalsExcelExporter productPricesForHospitalsExcelExporter, IRepository<Product, Guid> lookup_productRepository, IRepository<ProductCategory, Guid> lookup_productCategoryRepository)
        {
            _productPricesForHospitalRepository = productPricesForHospitalRepository;
            _productPricesForHospitalsExcelExporter = productPricesForHospitalsExcelExporter;
            _lookup_productRepository = lookup_productRepository;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;

        }

        public async Task<PagedResultDto<GetProductPricesForHospitalForViewDto>> GetAll(GetAllProductPricesForHospitalsInput input)
        {

            var filteredProductPricesForHospitals = _productPricesForHospitalRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ProductCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter);

            var pagedAndFilteredProductPricesForHospitals = filteredProductPricesForHospitals
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productPricesForHospitals = from o in pagedAndFilteredProductPricesForHospitals
                                            join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                                            from s1 in j1.DefaultIfEmpty()

                                            join o2 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o2.Id into j2
                                            from s2 in j2.DefaultIfEmpty()

                                            select new
                                            {

                                                o.StartDate,
                                                o.EndDate,
                                                o.Price,
                                                Id = o.Id,
                                                ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                                ProductCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                            };

            var totalCount = await filteredProductPricesForHospitals.CountAsync();

            var dbList = await productPricesForHospitals.ToListAsync();
            var results = new List<GetProductPricesForHospitalForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductPricesForHospitalForViewDto()
                {
                    ProductPricesForHospital = new ProductPricesForHospitalDto
                    {

                        StartDate = o.StartDate,
                        EndDate = o.EndDate,
                        Price = o.Price,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName,
                    ProductCategoryName = o.ProductCategoryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductPricesForHospitalForViewDto>(
                totalCount,
                results
            );

        }

        [AbpAuthorize(AppPermissions.Pages_ProductPricesForHospitals_Edit)]
        public async Task<GetProductPricesForHospitalForEditOutput> GetProductPricesForHospitalForEdit(EntityDto<Guid> input)
        {
            var productPricesForHospital = await _productPricesForHospitalRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductPricesForHospitalForEditOutput { ProductPricesForHospital = ObjectMapper.Map<CreateOrEditProductPricesForHospitalDto>(productPricesForHospital) };

            if (output.ProductPricesForHospital.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((Guid)output.ProductPricesForHospital.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            if (output.ProductPricesForHospital.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((Guid)output.ProductPricesForHospital.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductPricesForHospitalDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductPricesForHospitals_Create)]
        protected virtual async Task Create(CreateOrEditProductPricesForHospitalDto input)
        {
            var productPricesForHospital = ObjectMapper.Map<ProductPricesForHospital>(input);

            await _productPricesForHospitalRepository.InsertAsync(productPricesForHospital);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductPricesForHospitals_Edit)]
        protected virtual async Task Update(CreateOrEditProductPricesForHospitalDto input)
        {
            var productPricesForHospital = await _productPricesForHospitalRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, productPricesForHospital);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductPricesForHospitals_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _productPricesForHospitalRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductPricesForHospitalsToExcel(GetAllProductPricesForHospitalsForExcelInput input)
        {

            var filteredProductPricesForHospitals = _productPricesForHospitalRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .Include(e => e.ProductCategoryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinStartDateFilter != null, e => e.StartDate >= input.MinStartDateFilter)
                        .WhereIf(input.MaxStartDateFilter != null, e => e.StartDate <= input.MaxStartDateFilter)
                        .WhereIf(input.MinEndDateFilter != null, e => e.EndDate >= input.MinEndDateFilter)
                        .WhereIf(input.MaxEndDateFilter != null, e => e.EndDate <= input.MaxEndDateFilter)
                        .WhereIf(input.MinPriceFilter != null, e => e.Price >= input.MinPriceFilter)
                        .WhereIf(input.MaxPriceFilter != null, e => e.Price <= input.MaxPriceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter);

            var query = (from o in filteredProductPricesForHospitals
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProductPricesForHospitalForViewDto()
                         {
                             ProductPricesForHospital = new ProductPricesForHospitalDto
                             {
                                 StartDate = o.StartDate,
                                 EndDate = o.EndDate,
                                 Price = o.Price,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ProductCategoryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var productPricesForHospitalListDtos = await query.ToListAsync();

            return _productPricesForHospitalsExcelExporter.ExportToFile(productPricesForHospitalListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductPricesForHospitals)]
        public async Task<List<ProductPricesForHospitalProductLookupTableDto>> GetAllProductForTableDropdown()
        {
            return await _lookup_productRepository.GetAll()
                .Select(product => new ProductPricesForHospitalProductLookupTableDto
                {
                    Id = product.Id.ToString(),
                    DisplayName = product == null || product.Name == null ? "" : product.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_ProductPricesForHospitals)]
        public async Task<List<ProductPricesForHospitalProductCategoryLookupTableDto>> GetAllProductCategoryForTableDropdown()
        {
            return await _lookup_productCategoryRepository.GetAll()
                .Select(productCategory => new ProductPricesForHospitalProductCategoryLookupTableDto
                {
                    Id = productCategory.Id.ToString(),
                    DisplayName = productCategory == null || productCategory.Name == null ? "" : productCategory.Name.ToString()
                }).ToListAsync();
        }

    }
}