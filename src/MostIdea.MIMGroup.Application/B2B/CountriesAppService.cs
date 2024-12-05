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
    //[AbpAuthorize(AppPermissions.Pages_Countries)]
    public class CountriesAppService : MIMGroupAppServiceBase, ICountriesAppService
    {
        private readonly IRepository<Country, Guid> _countryRepository;
        private readonly ICountriesExcelExporter _countriesExcelExporter;

        public CountriesAppService(IRepository<Country, Guid> countryRepository, ICountriesExcelExporter countriesExcelExporter)
        {
            _countryRepository = countryRepository;
            _countriesExcelExporter = countriesExcelExporter;
        }

        public async Task<PagedResultDto<GetCountryForViewDto>> GetAll(GetAllCountriesInput input)
        {
            var filteredCountries = _countryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var pagedAndFilteredCountries = filteredCountries
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var countries = from o in pagedAndFilteredCountries
                            select new
                            {
                                o.Name,
                                Id = o.Id
                            };

            var totalCount = await filteredCountries.CountAsync();

            var dbList = await countries.ToListAsync();
            var results = new List<GetCountryForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetCountryForViewDto()
                {
                    Country = new CountryDto
                    {
                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetCountryForViewDto>(
                totalCount,
                results
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_Countries_Edit)]
        public async Task<GetCountryForEditOutput> GetCountryForEdit(EntityDto<Guid> input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetCountryForEditOutput { Country = ObjectMapper.Map<CreateOrEditCountryDto>(country) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditCountryDto input)
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

        //[AbpAuthorize(AppPermissions.Pages_Countries_Create)]
        protected virtual async Task Create(CreateOrEditCountryDto input)
        {
            var country = ObjectMapper.Map<Country>(input);

            await _countryRepository.InsertAsync(country);
        }

        //[AbpAuthorize(AppPermissions.Pages_Countries_Edit)]
        protected virtual async Task Update(CreateOrEditCountryDto input)
        {
            var country = await _countryRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, country);
        }

        //[AbpAuthorize(AppPermissions.Pages_Countries_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _countryRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetCountriesToExcel(GetAllCountriesForExcelInput input)
        {
            var filteredCountries = _countryRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var query = (from o in filteredCountries
                         select new GetCountryForViewDto()
                         {
                             Country = new CountryDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var countryListDtos = await query.ToListAsync();

            return _countriesExcelExporter.ExportToFile(countryListDtos);
        }
    }
}