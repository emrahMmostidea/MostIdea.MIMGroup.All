using Abp.Domain.Entities.Auditing;
using MostIdea.MIMGroup.Authorization.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Auditing;

namespace MostIdea.MIMGroup.B2B
{
    [Table("Orders")]
    [Audited]
    public class Order : FullAuditedEntity<Guid>
    {

        public virtual decimal Total { get; set; }

        public virtual decimal Tax { get; set; }

        public virtual decimal GrandTotal { get; set; }

        public virtual OrderStatusEnum Status { get; set; }

        public virtual OrderPaymentTypeEnum PaymentType { get; set; }

        public virtual OrderTypeEnum OrderType { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual string PatientName { get; set; }

        public virtual string PatientSurname { get; set; }

        public virtual Guid? DeliveryAddressId { get; set; }

        public virtual DateTime? OperationTime { get; set; }

        public virtual string VehiclePlate { get; set; }

        [ForeignKey("DeliveryAddressId")]
        public AddressInformation DeliveryAddressFk { get; set; }

        public virtual Guid? InvoiceAddressId { get; set; }

        [ForeignKey("InvoiceAddressId")]
        public AddressInformation InvoiceAddressFk { get; set; }

        public virtual long? CourierId { get; set; }

        [ForeignKey("CourierId")]
        public User CourierFk { get; set; }

        public virtual Guid? HospitalId { get; set; }

        [ForeignKey("HospitalId")]
        public Hospital HospitalFk { get; set; }

        public virtual long? DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public User DoctorFk { get; set; }

        public virtual Guid? WarehouseId { get; set; }

        [ForeignKey("WarehouseId")]
        public Warehouse WarehouseFk { get; set; }


        public virtual List<OrderItem> OrderItems { get; set; }

    }
}