using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.Organizations.Dto
{
    public class FindOrganizationUnitRolesInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}