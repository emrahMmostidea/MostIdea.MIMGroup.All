using System;
using System.ComponentModel.DataAnnotations;

namespace MostIdea.MIMGroup.B2B.Dtos
{
    public class OrderPageDto
    {

        public decimal Total { get; set; }

        public decimal Tax { get; set; }

        public decimal GrandTotal { get; set; }

        public OrderStatusEnum Status { get; set; }

        [Required]
        public string OrderNo { get; set; }

        public Guid AddressInformationId { get; set; }

        public long? CourierId { get; set; }

        public Guid HospitalId { get; set; }

        public long DoctorId { get; set; }

        public Guid? WarehouseId { get; set; }

        public CreateOrEditOrderDto Order { get; set; }

        public string AddressInformationName { get; set; }

        public string UserName { get; set; }

        public string HospitalName { get; set; }

        public string UserName2 { get; set; }

        public string WarehouseName { get; set; }
    }
}