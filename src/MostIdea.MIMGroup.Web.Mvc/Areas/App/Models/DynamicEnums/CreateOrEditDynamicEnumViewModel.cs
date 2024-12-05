using MostIdea.MIMGroup.B2B.Dtos;

using Abp.Extensions;
using System.Collections.Generic;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.DynamicEnums
{
    public class CreateOrEditDynamicEnumModalViewModel
    {
        public CreateOrEditDynamicEnumDto DynamicEnum { get; set; }

        public List<SelectionDto> Enums { get; set; }

        public bool IsEditMode => DynamicEnum.Id.HasValue;
    }
}