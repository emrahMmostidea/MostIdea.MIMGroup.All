using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using System.Collections.Generic;

namespace MostIdea.MIMGroup.B2B
{
    public interface IHospitalsAppService : IApplicationService
    {
        Task<PagedResultDto<GetHospitalForViewDto>> GetAll(GetAllHospitalsInput input);

        Task<GetHospitalForViewDto> GetHospitalForView(Guid id);

        Task<GetHospitalForEditOutput> GetHospitalForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditHospitalDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetHospitalsToExcel(GetAllHospitalsForExcelInput input);

        Task<List<HospitalHospitalGroupLookupTableDto>> GetAllHospitalGroupForTableDropdown();

        Task<List<SelectionDto>> Selection(SelectionInput input);

        Task<bool> SetSelectedHospital(Guid hospitalId);

        Task<HospitalDto> GetSelectedHospital();

        Task<bool> ClearSelectedHospital();

    }
}