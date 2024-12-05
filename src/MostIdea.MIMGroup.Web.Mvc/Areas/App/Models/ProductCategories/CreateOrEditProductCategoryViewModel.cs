using MostIdea.MIMGroup.B2B.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.ProductCategories
{
    public class CreateOrEditProductCategoryModalViewModel
    {
        public CreateOrEditProductCategoryDto ProductCategory { get; set; }

        public string ProductCategoryName { get; set; }

        public string BrandName { get; set; }

        public List<ProductCategoryProductCategoryLookupTableDto> ProductCategoryProductCategoryList { get; set; }

        public List<ProductCategoryBrandLookupTableDto> ProductCategoryBrandList { get; set; }

        public bool IsEditMode => ProductCategory.Id.HasValue;
    }
}