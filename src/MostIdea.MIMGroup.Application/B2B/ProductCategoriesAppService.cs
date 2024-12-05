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
    //[AbpAuthorize(AppPermissions.Pages_ProductCategories)]
    public class ProductCategoriesAppService : MIMGroupAppServiceBase, IProductCategoriesAppService
    {
        private readonly IRepository<ProductCategory, Guid> _productCategoryRepository;
        private readonly IProductCategoriesExcelExporter _productCategoriesExcelExporter;
        private readonly IRepository<ProductCategory, Guid> _lookup_productCategoryRepository;
        private readonly IRepository<Brand, Guid> _lookup_brandRepository;

        public ProductCategoriesAppService(IRepository<ProductCategory, Guid> productCategoryRepository, IProductCategoriesExcelExporter productCategoriesExcelExporter, IRepository<ProductCategory, Guid> lookup_productCategoryRepository, IRepository<Brand, Guid> lookup_brandRepository)
        {
            _productCategoryRepository = productCategoryRepository;
            _productCategoriesExcelExporter = productCategoriesExcelExporter;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;
            _lookup_brandRepository = lookup_brandRepository;
        }

        public async Task<PagedResultDto<GetProductCategoryForViewDto>> GetAll(GetAllProductCategoriesInput input)
        {
            var filteredProductCategories = _productCategoryRepository.GetAll()
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.BrandFk)
                        .Include(e => e.ChildCategories)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(input.ParentId.HasValue, x => x.ProductCategoryId.Value == input.ParentId)
                        .WhereIf(!input.ParentId.HasValue, x => !x.ProductCategoryId.HasValue)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BrandNameFilter), e => e.BrandFk != null && e.BrandFk.Name == input.BrandNameFilter);

            var pagedAndFilteredProductCategories = filteredProductCategories
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productCategories = from o in pagedAndFilteredProductCategories
                                    join o1 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
                                    from s1 in j1.DefaultIfEmpty()

                                    join o2 in _lookup_brandRepository.GetAll() on o.BrandId equals o2.Id into j2
                                    from s2 in j2.DefaultIfEmpty()


                                    select new
                                    {
                                        o.Name,
                                        o.Description,
                                        Id = o.Id,
                                        ProductCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                        BrandName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                                        ChildCount = o.ChildCategories.Count
                                    };

            var totalCount = await filteredProductCategories.CountAsync();

            var dbList = await productCategories.ToListAsync();
            var results = new List<GetProductCategoryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductCategoryForViewDto()
                {
                    ProductCategory = new ProductCategoryDto
                    {
                        Name = o.Name,
                        Description = o.Description,
                        Id = o.Id,  
                        ChildCount = o.ChildCount,
                    },
                    ProductCategoryName = o.ProductCategoryName,
                    BrandName = o.BrandName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductCategoryForViewDto>(
                totalCount,
                results
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_ProductCategories_Edit)]
        public async Task<GetProductCategoryForEditOutput> GetProductCategoryForEdit(EntityDto<Guid> input)
        {
            var productCategory = await _productCategoryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductCategoryForEditOutput { ProductCategory = ObjectMapper.Map<CreateOrEditProductCategoryDto>(productCategory) };

            if (output.ProductCategory.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((Guid)output.ProductCategory.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.ProductCategory.BrandId != null)
            {
                var _lookupBrand = await _lookup_brandRepository.FirstOrDefaultAsync((Guid)output.ProductCategory.BrandId);
                output.BrandName = _lookupBrand?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductCategoryDto input)
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

        //[AbpAuthorize(AppPermissions.Pages_ProductCategories_Create)]
        protected virtual async Task Create(CreateOrEditProductCategoryDto input)
        {
            if (input.ProductCategoryId.HasValue)
            {
                var parent = await _lookup_productCategoryRepository.FirstOrDefaultAsync(input.ProductCategoryId.Value);
                input.BrandId = parent.BrandId;
            }
            var productCategory = ObjectMapper.Map<ProductCategory>(input);

            await _productCategoryRepository.InsertAsync(productCategory);
        }

        //[AbpAuthorize(AppPermissions.Pages_ProductCategories_Edit)]
        protected virtual async Task Update(CreateOrEditProductCategoryDto input)
        {
            var productCategory = await _productCategoryRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, productCategory);
        }

        //[AbpAuthorize(AppPermissions.Pages_ProductCategories_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _productCategoryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductCategoriesToExcel(GetAllProductCategoriesForExcelInput input)
        {
            var filteredProductCategories = _productCategoryRepository.GetAll()
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.BrandFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BrandNameFilter), e => e.BrandFk != null && e.BrandFk.Name == input.BrandNameFilter);

            var query = (from o in filteredProductCategories
                         join o1 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_brandRepository.GetAll() on o.BrandId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProductCategoryForViewDto()
                         {
                             ProductCategory = new ProductCategoryDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Id = o.Id
                             },
                             ProductCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             BrandName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var productCategoryListDtos = await query.ToListAsync();

            return _productCategoriesExcelExporter.ExportToFile(productCategoryListDtos);
        }

        //[AbpAuthorize(AppPermissions.Pages_ProductCategories)]
        public async Task<List<ProductCategoryProductCategoryLookupTableDto>> GetAllProductCategoryForTableDropdown()
        {
            return await _lookup_productCategoryRepository.GetAll()
                .Select(productCategory => new ProductCategoryProductCategoryLookupTableDto
                {
                    Id = productCategory.Id.ToString(),
                    DisplayName = productCategory == null || productCategory.Name == null ? "" : productCategory.Name.ToString()
                }).ToListAsync();
        }

        //[AbpAuthorize(AppPermissions.Pages_ProductCategories)]
        public async Task<List<ProductCategoryBrandLookupTableDto>> GetAllBrandForTableDropdown()
        {
            return await _lookup_brandRepository.GetAll()
                .Select(brand => new ProductCategoryBrandLookupTableDto
                {
                    Id = brand.Id.ToString(),
                    DisplayName = brand == null || brand.Name == null ? "" : brand.Name.ToString()
                }).ToListAsync();
        }
    }
}