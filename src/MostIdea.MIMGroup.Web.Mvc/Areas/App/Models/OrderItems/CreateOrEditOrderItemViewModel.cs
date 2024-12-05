using MostIdea.MIMGroup.B2B.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.OrderItems
{
    public class CreateOrEditOrderItemModalViewModel
    {
        public CreateOrEditOrderItemDto OrderItem { get; set; }

        public string ProductName { get; set; }

        public string OrderOrderNo { get; set; }

        public List<OrderItemProductLookupTableDto> OrderItemProductList { get; set; }

        public List<OrderItemOrderLookupTableDto> OrderItemOrderList { get; set; }

        public bool IsEditMode => OrderItem.Id.HasValue;
    }
}