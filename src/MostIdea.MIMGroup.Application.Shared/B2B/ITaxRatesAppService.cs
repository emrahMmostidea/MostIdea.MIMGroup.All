using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.B2B
{
    public interface ITaxRatesAppService : IApplicationService
    {
        Task<PagedResultDto<GetTaxRateForViewDto>> GetAll(GetAllTaxRatesInput input);

        Task<GetTaxRateForEditOutput> GetTaxRateForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditTaxRateDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetTaxRatesToExcel(GetAllTaxRatesForExcelInput input);

    }
}