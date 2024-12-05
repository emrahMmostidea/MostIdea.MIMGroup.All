using System.Collections.Generic;
using MostIdea.MIMGroup.DynamicEntityProperties.Dto;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.DynamicEntityProperty
{
    public class CreateEntityDynamicPropertyViewModel
    {
        public string EntityFullName { get; set; }

        public List<string> AllEntities { get; set; }

        public List<DynamicPropertyDto> DynamicProperties { get; set; }
    }
}
