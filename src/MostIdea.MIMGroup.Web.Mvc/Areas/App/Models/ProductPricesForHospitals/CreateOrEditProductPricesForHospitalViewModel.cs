using MostIdea.MIMGroup.B2B.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.ProductPricesForHospitals
{
    public class CreateOrEditProductPricesForHospitalModalViewModel
    {
        public CreateOrEditProductPricesForHospitalDto ProductPricesForHospital { get; set; }

        public string ProductName { get; set; }

        public string ProductCategoryName { get; set; }

        public List<ProductPricesForHospitalProductLookupTableDto> ProductPricesForHospitalProductList { get; set; }

        public List<ProductPricesForHospitalProductCategoryLookupTableDto> ProductPricesForHospitalProductCategoryList { get; set; }

        public bool IsEditMode => ProductPricesForHospital.Id.HasValue;
    }
}