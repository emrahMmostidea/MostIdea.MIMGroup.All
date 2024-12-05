using Abp.AutoMapper;
using MostIdea.MIMGroup.Organizations.Dto;

namespace MostIdea.MIMGroup.Models.Users
{
    [AutoMapFrom(typeof(OrganizationUnitDto))]
    public class OrganizationUnitModel : OrganizationUnitDto
    {
        public bool IsAssigned { get; set; }
    }
}