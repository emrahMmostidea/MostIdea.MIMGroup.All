using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.B2B
{
    public interface IBrandsAppService : IApplicationService
    {
        Task<PagedResultDto<GetBrandForViewDto>> GetAll(GetAllBrandsInput input);

        Task<GetBrandForEditOutput> GetBrandForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditBrandDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetBrandsToExcel(GetAllBrandsForExcelInput input);

    }
}