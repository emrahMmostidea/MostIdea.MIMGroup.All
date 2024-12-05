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
    //[AbpAuthorize(AppPermissions.Pages_TaxRates)]
    public class TaxRatesAppService : MIMGroupAppServiceBase, ITaxRatesAppService
    {
        private readonly IRepository<TaxRate, Guid> _taxRateRepository;
        private readonly ITaxRatesExcelExporter _taxRatesExcelExporter;

        public TaxRatesAppService(IRepository<TaxRate, Guid> taxRateRepository, ITaxRatesExcelExporter taxRatesExcelExporter)
        {
            _taxRateRepository = taxRateRepository;
            _taxRatesExcelExporter = taxRatesExcelExporter;
        }

        public async Task<PagedResultDto<GetTaxRateForViewDto>> GetAll(GetAllTaxRatesInput input)
        {
            var filteredTaxRates = _taxRateRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(input.MinRateFilter != null, e => e.Rate >= input.MinRateFilter)
                        .WhereIf(input.MaxRateFilter != null, e => e.Rate <= input.MaxRateFilter);

            var pagedAndFilteredTaxRates = filteredTaxRates
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var taxRates = from o in pagedAndFilteredTaxRates
                           select new
                           {
                               o.Name,
                               o.Rate,
                               Id = o.Id
                           };

            var totalCount = await filteredTaxRates.CountAsync();

            var dbList = await taxRates.ToListAsync();
            var results = new List<GetTaxRateForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetTaxRateForViewDto()
                {
                    TaxRate = new TaxRateDto
                    {
                        Name = o.Name,
                        Rate = o.Rate,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetTaxRateForViewDto>(
                totalCount,
                results
            );
        }

        //[AbpAuthorize(AppPermissions.Pages_TaxRates_Edit)]
        public async Task<GetTaxRateForEditOutput> GetTaxRateForEdit(EntityDto<Guid> input)
        {
            var taxRate = await _taxRateRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetTaxRateForEditOutput { TaxRate = ObjectMapper.Map<CreateOrEditTaxRateDto>(taxRate) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditTaxRateDto input)
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

        //[AbpAuthorize(AppPermissions.Pages_TaxRates_Create)]
        protected virtual async Task Create(CreateOrEditTaxRateDto input)
        {
            var taxRate = ObjectMapper.Map<TaxRate>(input);

            await _taxRateRepository.InsertAsync(taxRate);
        }

        //[AbpAuthorize(AppPermissions.Pages_TaxRates_Edit)]
        protected virtual async Task Update(CreateOrEditTaxRateDto input)
        {
            var taxRate = await _taxRateRepository.FirstOrDefaultAsync((Guid)input.Id);
            ObjectMapper.Map(input, taxRate);
        }

        //[AbpAuthorize(AppPermissions.Pages_TaxRates_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _taxRateRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetTaxRatesToExcel(GetAllTaxRatesForExcelInput input)
        {
            var filteredTaxRates = _taxRateRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                        .WhereIf(input.MinRateFilter != null, e => e.Rate >= input.MinRateFilter)
                        .WhereIf(input.MaxRateFilter != null, e => e.Rate <= input.MaxRateFilter);

            var query = (from o in filteredTaxRates
                         select new GetTaxRateForViewDto()
                         {
                             TaxRate = new TaxRateDto
                             {
                                 Name = o.Name,
                                 Rate = o.Rate,
                                 Id = o.Id
                             }
                         });

            var taxRateListDtos = await query.ToListAsync();

            return _taxRatesExcelExporter.ExportToFile(taxRateListDtos);
        }
    }
}