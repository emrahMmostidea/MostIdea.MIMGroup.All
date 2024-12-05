using Abp.Application.Services.Dto;
using System;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAllTaxRatesInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }

        public decimal? MaxRateFilter { get; set; }
        public decimal? MinRateFilter { get; set; }

    }
}