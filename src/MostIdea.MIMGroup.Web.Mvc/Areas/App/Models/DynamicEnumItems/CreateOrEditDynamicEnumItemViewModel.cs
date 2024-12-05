using MostIdea.MIMGroup.B2B.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.DynamicEnumItems
{
    public class CreateOrEditDynamicEnumItemModalViewModel
    {
        public CreateOrEditDynamicEnumItemDto DynamicEnumItem { get; set; }
        
        public CreateOrEditDynamicEnumDto DynamicEnum { get; set; }

        public string DynamicEnumName { get; set; }

        public List<DynamicEnumItemDynamicEnumLookupTableDto> DynamicEnumItemDynamicEnumList { get; set; }

        public bool IsEditMode => DynamicEnumItem.Id.HasValue;
    }
}