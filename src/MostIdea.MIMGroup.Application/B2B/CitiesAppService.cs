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
    //[AbpAuthorize(AppPermissions.Pages_Cities)]
    public class CitiesAppService : MIMGroupAppServiceBase, ICitiesAppService
    {
        private readonly IRepository<City, Guid> _cityRepository;
        private readonly ICitiesExcelExporter _citiesExcelExporter;
        private readonly IRepository<Country, Guid> _lookup_countryRepository;

        public CitiesAppService(IRepository<City, Guid> cityRepository, ICitiesExcelExporter citiesExcelExporter, IRepository<Country, Guid> lookup_countryRepository)
        {
            _cityRepository = cityRepository;
            _citiesExcelExporter = citiesExcelExporter;
            _lookup_countryRepository = lookup_countryRepository;
        }

        public async Task<PagedResultDto<GetCityForViewDto>> GetAll(GetAllCitiesInput input)
        {
            var filteredCities = _cityRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter);

            var pagedAndFilteredCities = filteredCities
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var cities = from o in pagedAndFilteredCities
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new
                         {
                             o.Name,
                             Id = o.Id,
                             CountryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         };

            var totalCount = await filteredCities.CountAsync();

            var dbList = await cities.ToListAsync();
            var results = new List<GetCityForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCityForViewDto()
                {
                    City = new CityDto
                    {
                        Name = o.Name,
                        Id = o.Id,
                    },
                    CountryName = o.CountryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCityForViewDto>(
                totalCount,
                results
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_Cities_Edit)]
        public async Task<GetCityForEditOutput> GetCityForEdit(EntityDto<Guid> input)
        {
            var city = await _cityRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCityForEditOutput { City = ObjectMapper.Map<CreateOrEditCityDto>(city) };

            if (output.City.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((Guid)output.City.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCityDto input)
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

        //[AbpAuthorize(AppPermissions.Pages_Cities_Create)]
        protected virtual async Task Create(CreateOrEditCityDto input)
        {
            var city = ObjectMapper.Map<City>(input);

            await _cityRepository.InsertAsync(city);
        }

        //[AbpAuthorize(AppPermissions.Pages_Cities_Edit)]
        protected virtual async Task Update(CreateOrEditCityDto input)
        {
            var city = await _cityRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, city);
        }

        //[AbpAuthorize(AppPermissions.Pages_Cities_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _cityRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCitiesToExcel(GetAllCitiesForExcelInput input)
        {
            var filteredCities = _cityRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter);

            var query = (from o in filteredCities
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetCityForViewDto()
                         {
                             City = new CityDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             },
                             CountryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var cityListDtos = await query.ToListAsync();

            return _citiesExcelExporter.ExportToFile(cityListDtos);
        }

        //[AbpAuthorize(AppPermissions.Pages_Cities)]
        public async Task<List<CityCountryLookupTableDto>> GetAllCountryForTableDropdown()
        {
            return await _lookup_countryRepository.GetAll()
                .Select(country => new CityCountryLookupTableDto
                {
                    Id = country.Id.ToString(),
                    DisplayName = country == null || country.Name == null ? "" : country.Name.ToString()
                }).ToListAsync();
        }
    }
}