using System.Collections.Generic;
using MostIdea.MIMGroup.Organizations.Dto;

namespace MostIdea.MIMGroup.Web.Areas.App.Models.Common
{
    public interface IOrganizationUnitsEditViewModel
    {
        List<OrganizationUnitDto> AllOrganizationUnits { get; set; }

        List<string> MemberedOrganizationUnits { get; set; }
    }
}