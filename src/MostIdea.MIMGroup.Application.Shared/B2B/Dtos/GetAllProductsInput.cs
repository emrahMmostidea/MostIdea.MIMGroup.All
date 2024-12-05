using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAllProductsInput : PagedAndSortedResultRequestDto
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
        
        public Guid? CategoryId { get; set; }

        public List<AttributeDto> AttributeFilters { get; set; }

    }

    // TODO : Taşınacak Product Attribute yapısı oluşturulacak
    public class AttributeDto
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public string Icon { get; set; }

        public string Color { get; set; }
    }
}