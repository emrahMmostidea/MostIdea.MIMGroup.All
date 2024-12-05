using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using System.Collections.Generic;
using System.Collections.Generic;

namespace MostIdea.MIMGroup.B2B
{
    public interface IHospitalVsUsersAppService : IApplicationService
    {
        Task<PagedResultDto<GetHospitalVsUserForViewDto>> GetAll(GetAllHospitalVsUsersInput input);

        Task<GetHospitalVsUserForEditOutput> GetHospitalVsUserForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditHospitalVsUserDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetHospitalVsUsersToExcel(GetAllHospitalVsUsersForExcelInput input);

        Task<List<HospitalVsUserHospitalLookupTableDto>> GetAllHospitalForTableDropdown();

        Task<List<HospitalVsUserUserLookupTableDto>> GetAllUserForTableDropdown();

    }
}