using Abp.Application.Services.Dto;
using System;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAllProductPricesForHospitalsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public DateTime? MaxStartDateFilter { get; set; }
        public DateTime? MinStartDateFilter { get; set; }

        public DateTime? MaxEndDateFilter { get; set; }
        public DateTime? MinEndDateFilter { get; set; }

        public decimal? MaxPriceFilter { get; set; }
        public decimal? MinPriceFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string ProductCategoryNameFilter { get; set; }

    }
}