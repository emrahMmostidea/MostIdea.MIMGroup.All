using MostIdea.MIMGroup.B2B.Dtos;
using System.Collections.Generic;
using System.Collections.Generic;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.WarehouseVsCouriers
{
    public class CreateOrEditWarehouseVsCourierModalViewModel
    {
        public CreateOrEditWarehouseVsCourierDto WarehouseVsCourier { get; set; }

        public string UserName { get; set; }

        public string WarehouseName { get; set; }

        public List<WarehouseVsCourierUserLookupTableDto> WarehouseVsCourierUserList { get; set; }

        public List<WarehouseVsCourierWarehouseLookupTableDto> WarehouseVsCourierWarehouseList { get; set; }

        public bool IsEditMode => WarehouseVsCourier.Id.HasValue;
    }
}