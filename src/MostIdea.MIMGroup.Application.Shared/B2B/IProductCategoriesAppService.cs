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
    public interface IProductCategoriesAppService : IApplicationService
    {
        Task<PagedResultDto<GetProductCategoryForViewDto>> GetAll(GetAllProductCategoriesInput input);

        Task<GetProductCategoryForEditOutput> GetProductCategoryForEdit(EntityDto<Guid> input);

        Task CreateOrEdit(CreateOrEditProductCategoryDto input);

        Task Delete(EntityDto<Guid> input);

        Task<FileDto> GetProductCategoriesToExcel(GetAllProductCategoriesForExcelInput input);

        Task<List<ProductCategoryProductCategoryLookupTableDto>> GetAllProductCategoryForTableDropdown();

        Task<List<ProductCategoryBrandLookupTableDto>> GetAllBrandForTableDropdown();

    }
}