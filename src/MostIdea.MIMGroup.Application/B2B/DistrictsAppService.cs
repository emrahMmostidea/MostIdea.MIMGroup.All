using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
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
    //[AbpAuthorize(AppPermissions.Pages_Districts)]
    public class DistrictsAppService : MIMGroupAppServiceBase, IDistrictsAppService
    {
        private readonly IRepository<District, Guid> _districtRepository;
        private readonly IDistrictsExcelExporter _districtsExcelExporter;
        private readonly IRepository<City, Guid> _lookup_cityRepository;

        public DistrictsAppService(IRepository<District, Guid> districtRepository, IDistrictsExcelExporter districtsExcelExporter, IRepository<City, Guid> lookup_cityRepository)
        {
            _districtRepository = districtRepository;
            _districtsExcelExporter = districtsExcelExporter;
            _lookup_cityRepository = lookup_cityRepository;
        }

        public async Task<PagedResultDto<GetDistrictForViewDto>> GetAll(GetAllDistrictsInput input)
        {
            var filteredDistricts = _districtRepository.GetAll()
                        .Include(e => e.CityFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityNameFilter), e => e.CityFk != null && e.CityFk.Name == input.CityNameFilter);

            var pagedAndFilteredDistricts = filteredDistricts
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var districts = from o in pagedAndFilteredDistricts
                            join o1 in _lookup_cityRepository.GetAll() on o.CityId equals o1.Id into j1
                            from s1 in j1.DefaultIfEmpty()

                            select new
                            {
                                o.Name,
                                Id = o.Id,
                                CityName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                            };

            var totalCount = await filteredDistricts.CountAsync();

            var dbList = await districts.ToListAsync();
            var results = new List<GetDistrictForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetDistrictForViewDto()
                {
                    District = new DistrictDto
                    {
                        Name = o.Name,
                        Id = o.Id,
                    },
                    CityName = o.CityName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetDistrictForViewDto>(
                totalCount,
                results
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_Districts_Edit)]
        public async Task<GetDistrictForEditOutput> GetDistrictForEdit(EntityDto<Guid> input)
        {
            var district = await _districtRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDistrictForEditOutput { District = ObjectMapper.Map<CreateOrEditDistrictDto>(district) };

            if (output.District.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((Guid)output.District.CityId);
                output.CityName = _lookupCity?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDistrictDto input)
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

        //[AbpAuthorize(AppPermissions.Pages_Districts_Create)]
        protected virtual async Task Create(CreateOrEditDistrictDto input)
        {
            var district = ObjectMapper.Map<District>(input);

            await _districtRepository.InsertAsync(district);
        }

        //[AbpAuthorize(AppPermissions.Pages_Districts_Edit)]
        protected virtual async Task Update(CreateOrEditDistrictDto input)
        {
            var district = await _districtRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, district);
        }

        //[AbpAuthorize(AppPermissions.Pages_Districts_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _districtRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDistrictsToExcel(GetAllDistrictsForExcelInput input)
        {
            var filteredDistricts = _districtRepository.GetAll()
                        .Include(e => e.CityFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityNameFilter), e => e.CityFk != null && e.CityFk.Name == input.CityNameFilter);

            var query = (from o in filteredDistricts
                         join o1 in _lookup_cityRepository.GetAll() on o.CityId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetDistrictForViewDto()
                         {
                             District = new DistrictDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             },
                             CityName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var districtListDtos = await query.ToListAsync();

            return _districtsExcelExporter.ExportToFile(districtListDtos);
        }

        //[AbpAuthorize(AppPermissions.Pages_Districts)]
        public async Task<List<DistrictCityLookupTableDto>> GetAllCityForTableDropdown()
        {
            return await _lookup_cityRepository.GetAll()
                .Select(city => new DistrictCityLookupTableDto
                {
                    Id = city.Id.ToString(),
                    DisplayName = city == null || city.Name == null ? "" : city.Name.ToString()
                }).ToListAsync();
        }
    }
}