using MostIdea.MIMGroup.B2B.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Products
{
    public class CreateOrEditProductModalViewModel
    {
        public CreateOrEditProductDto Product { get; set; }

        public string ProductCategoryName { get; set; }

        public string TaxRateName { get; set; }

        public List<ProductProductCategoryLookupTableDto> ProductProductCategoryList { get; set; }

        public List<ProductTaxRateLookupTableDto> ProductTaxRateList { get; set; }

        public bool IsEditMode => Product.Id.HasValue;
    }
}