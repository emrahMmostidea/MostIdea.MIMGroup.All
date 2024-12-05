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
    //[AbpAuthorize(AppPermissions.Pages_Brands)]
    public class BrandsAppService : MIMGroupAppServiceBase, IBrandsAppService
    {
        private readonly IRepository<Brand, Guid> _brandRepository;
        private readonly IBrandsExcelExporter _brandsExcelExporter;

        public BrandsAppService(IRepository<Brand, Guid> brandRepository, IBrandsExcelExporter brandsExcelExporter)
        {
            _brandRepository = brandRepository;
            _brandsExcelExporter = brandsExcelExporter;
        }

        public async Task<PagedResultDto<GetBrandForViewDto>> GetAll(GetAllBrandsInput input)
        {
            var filteredBrands = _brandRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var pagedAndFilteredBrands = filteredBrands
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var brands = from o in pagedAndFilteredBrands
                         select new
                         {
                             o.Name,
                             o.Description,
                             Id = o.Id
                         };

            var totalCount = await filteredBrands.CountAsync();

            var dbList = await brands.ToListAsync();
            var results = new List<GetBrandForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetBrandForViewDto()
                {
                    Brand = new BrandDto
                    {
                        Name = o.Name,
                        Description = o.Description,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetBrandForViewDto>(
                totalCount,
                results
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_Brands_Edit)]
        public async Task<GetBrandForEditOutput> GetBrandForEdit(EntityDto<Guid> input)
        {
            var brand = await _brandRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetBrandForEditOutput { Brand = ObjectMapper.Map<CreateOrEditBrandDto>(brand) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditBrandDto input)
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

        //[AbpAuthorize(AppPermissions.Pages_Brands_Create)]
        protected virtual async Task Create(CreateOrEditBrandDto input)
        {
            var brand = ObjectMapper.Map<Brand>(input);

            await _brandRepository.InsertAsync(brand);
        }

        //[AbpAuthorize(AppPermissions.Pages_Brands_Edit)]
        protected virtual async Task Update(CreateOrEditBrandDto input)
        {
            var brand = await _brandRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, brand);
        }

        //[AbpAuthorize(AppPermissions.Pages_Brands_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _brandRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetBrandsToExcel(GetAllBrandsForExcelInput input)
        {
            var filteredBrands = _brandRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description == input.DescriptionFilter);

            var query = (from o in filteredBrands
                         select new GetBrandForViewDto()
                         {
                             Brand = new BrandDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 Id = o.Id
                             }
                         });

            var brandListDtos = await query.ToListAsync();

            return _brandsExcelExporter.ExportToFile(brandListDtos);
        }
    }
}