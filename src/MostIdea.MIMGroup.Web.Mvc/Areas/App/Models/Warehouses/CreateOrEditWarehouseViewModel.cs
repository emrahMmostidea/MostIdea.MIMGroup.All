using MostIdea.MIMGroup.B2B.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Warehouses
{
    public class CreateOrEditWarehouseModalViewModel
    {
        public CreateOrEditWarehouseDto Warehouse { get; set; }

        public string DistrictName { get; set; }

        public List<WarehouseDistrictLookupTableDto> WarehouseDistrictList { get; set; }

        public bool IsEditMode => Warehouse.Id.HasValue;
    }
}