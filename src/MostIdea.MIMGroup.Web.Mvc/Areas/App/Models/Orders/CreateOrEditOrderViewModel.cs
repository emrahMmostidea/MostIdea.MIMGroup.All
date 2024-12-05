using MostIdea.MIMGroup.B2B.Dtos;
using System.Collections.Generic;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Orders
{
    public class CreateOrEditOrderModalViewModel
    {
        public CreateOrEditOrderDto Order { get; set; }

        public string AddressInformationName { get; set; }

        public string UserName { get; set; }

        public string HospitalName { get; set; }

        public string UserName2 { get; set; }

        public string WarehouseName { get; set; }

        public List<OrderAddressInformationLookupTableDto> OrderAddressInformationList { get; set; }

        public List<OrderUserLookupTableDto> OrderUserList { get; set; }

        public List<OrderHospitalLookupTableDto> OrderHospitalList { get; set; }

        public List<OrderWarehouseLookupTableDto> OrderWarehouseList { get; set; }

        public bool IsEditMode => Order.Id.HasValue;
    }
}