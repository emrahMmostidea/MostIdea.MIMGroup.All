using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.B2B
{
    public interface ISalesConsultantsAppService : IApplicationService
    {
        Task<PagedResultDto<GetSalesConsultantForViewDto>> GetAll(GetAllSalesConsultantsInput input);

        Task<GetSalesConsultantForViewDto> GetSalesConsultantForView(Guid id);

        Task<GetSalesConsultantForEditOutput> GetSalesConsultantForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditSalesConsultantDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetSalesConsultantsToExcel(GetAllSalesConsultantsForExcelInput input);

        Task<PagedResultDto<SalesConsultantUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

    }
}