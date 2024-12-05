using System.Collections.Generic;
using MostIdea.MIMGroup.Editions.Dto;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Tenants
{
    public class TenantIndexViewModel
    {
        public List<SubscribableEditionComboboxItemDto> EditionItems { get; set; }
    }
}