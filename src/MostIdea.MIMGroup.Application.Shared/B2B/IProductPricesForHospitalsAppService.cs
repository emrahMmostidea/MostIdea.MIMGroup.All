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
    public interface IProductPricesForHospitalsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductPricesForHospitalForViewDto>> GetAll(GetAllProductPricesForHospitalsInput input);

        Task<GetProductPricesForHospitalForEditOutput> GetProductPricesForHospitalForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditProductPricesForHospitalDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetProductPricesForHospitalsToExcel(GetAllProductPricesForHospitalsForExcelInput input);

        Task<List<ProductPricesForHospitalProductLookupTableDto>> GetAllProductForTableDropdown();

        Task<List<ProductPricesForHospitalProductCategoryLookupTableDto>> GetAllProductCategoryForTableDropdown();

    }
}