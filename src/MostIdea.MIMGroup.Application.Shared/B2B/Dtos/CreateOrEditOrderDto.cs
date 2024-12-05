using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class CreateOrEditOrderDto : EntityDto<Guid?>
    {
        public string OrderNo { get; set; }

        public Guid? HospitalId { get; set; }

        public long? DoctorId { get; set; }

        public Guid? DeliveryAddressId { get; set; }

        public Guid? InvoiceAddressId { get; set; }

        public OrderStatusEnum Status { get; set; }

        public OrderPaymentTypeEnum PaymentType { get; set; }

        public string PatientName { get; set; }

        public string PatientSurname { get; set; }

        public DateTime? OperationTime { get; set; }

        public List<CreateOrEditOrderItemDto> OrderItems { get; set; }

        public long? CourierId { get; set; }

        public Guid? WarehouseId { get; set; }
        public virtual OrderTypeEnum OrderType { get; set; }

        public string Comment { get; set; }

        public DateTime? CreationTime { get; set; }
    }
}