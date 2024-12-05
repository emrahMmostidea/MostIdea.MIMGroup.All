using MostIdea.MIMGroup.B2B;

using System;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class OrderItemDto : EntityDto<Guid>
    {
        public decimal Price { get; set; }

        public int Amount { get; set; }

        public OrderItemStatusEnum Status { get; set; }

        public Guid ProductId { get; set; }

        public Guid OrderId { get; set; }

        public ProductDto Product { get; set; }
    }
}