using System.Collections.Generic;
using MostIdea.MIMGroup.DynamicEntityProperties.Dto;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.DynamicProperty
{
    public class CreateOrEditDynamicPropertyViewModel
    {
        public DynamicPropertyDto DynamicPropertyDto { get; set; }

        public List<string> AllowedInputTypes { get; set; }
    }
}
