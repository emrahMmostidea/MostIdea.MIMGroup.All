using MostIdea.MIMGroup.B2B;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditOrderItemDto : EntityDto<Guid?>
    {

        public decimal Price { get; set; }

        public int Amount { get; set; }

        public OrderItemStatusEnum Status { get; set; }

        public Guid ProductId { get; set; }

        public Guid? OrderId { get; set; }

        public Guid? HospitalId  { get; set; }

        public Guid? InvoiceId { get; set; }

        public Guid? DeliveryId{ get; set; }

    }
}