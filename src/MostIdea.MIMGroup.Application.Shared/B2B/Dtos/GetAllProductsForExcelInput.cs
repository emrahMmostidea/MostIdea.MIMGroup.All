using Abp.Application.Services.Dto;
using System;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAllProductsForExcelInput
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public string DescriptionFilter { get; set; }

        public decimal? MaxPriceFilter { get; set; }
        public decimal? MinPriceFilter { get; set; }

        public int? MaxQuantityFilter { get; set; }
        public int? MinQuantityFilter { get; set; }

        public string ProductCategoryNameFilter { get; set; }

        public string TaxRateNameFilter { get; set; }

    }
}