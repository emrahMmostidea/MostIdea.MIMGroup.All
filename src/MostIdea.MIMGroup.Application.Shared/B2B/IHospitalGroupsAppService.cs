using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.B2B
{
    public interface IHospitalGroupsAppService : IApplicationService
    {
        Task<PagedResultDto<GetHospitalGroupForViewDto>> GetAll(GetAllHospitalGroupsInput input);

        Task<GetHospitalGroupForEditOutput> GetHospitalGroupForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditHospitalGroupDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetHospitalGroupsToExcel(GetAllHospitalGroupsForExcelInput input);

    }
}