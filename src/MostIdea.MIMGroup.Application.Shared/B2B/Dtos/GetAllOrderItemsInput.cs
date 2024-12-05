using Abp.Application.Services.Dto;
using System;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class GetAllOrderItemsInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public decimal? MaxPriceFilter { get; set; }
        public decimal? MinPriceFilter { get; set; }

        public int? MaxAmountFilter { get; set; }
        public int? MinAmountFilter { get; set; }

        public int? StatusFilter { get; set; }

        public string ProductNameFilter { get; set; }

        public string OrderOrderNoFilter { get; set; }

        public Guid? OrderId { get; set; }

    }
}