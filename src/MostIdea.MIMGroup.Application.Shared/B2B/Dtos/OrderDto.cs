using MostIdea.MIMGroup.B2B;

using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class OrderDto : EntityDto<Guid>
    {
        public decimal Total { get; set; }

        public decimal Tax { get; set; }

        public decimal GrandTotal { get; set; }

        public OrderStatusEnum Status { get; set; }

        public OrderPaymentTypeEnum PaymentType { get; set; }
        public OrderTypeEnum OrderType { get; set; }

        public string OrderNo { get; set; }

        public Guid AddressInformationId { get; set; }

        public long? CourierId { get; set; }

        public Guid HospitalId { get; set; }

        public long DoctorId { get; set; }

        public Guid? WarehouseId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string PatientName { get; set; }

        public string PatientSurname { get; set; }

        public DateTime? OperationTime { get; set; }


        public List<OrderItemDto> Products { get; set; }

        public List<OrderCommentDto> Comments { get; set; }

    }
}