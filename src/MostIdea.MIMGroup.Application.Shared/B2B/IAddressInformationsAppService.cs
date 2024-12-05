using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.B2B
{
    public interface IAddressInformationsAppService : IApplicationService
    {
        Task<PagedResultDto<GetAddressInformationForViewDto>> GetAll(GetAllAddressInformationsInput input);

        Task<GetAddressInformationForEditOutput> GetAddressInformationForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditAddressInformationDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetAddressInformationsToExcel(GetAllAddressInformationsForExcelInput input);

    }
}