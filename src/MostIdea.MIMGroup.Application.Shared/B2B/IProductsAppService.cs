using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using MostIdea.MIMGroup.B2B.Dtos;
using MostIdea.MIMGroup.Dto;
using System.Collections.Generic; 

namespace MostIdea.MIMGroup.B2B
{
    public interface IProductsAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductForViewDto>> GetAll(GetAllProductsInput input);

        Task<GetProductForViewDto> GetProductForView(Guid id);

        Task<GetProductForEditOutput> GetProductForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditProductDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetProductsToExcel(GetAllProductsForExcelInput input);

        Task<List<ProductProductCategoryLookupTableDto>> GetAllProductCategoryForTableDropdown();

        Task<List<ProductTaxRateLookupTableDto>> GetAllTaxRateForTableDropdown();

    }
}