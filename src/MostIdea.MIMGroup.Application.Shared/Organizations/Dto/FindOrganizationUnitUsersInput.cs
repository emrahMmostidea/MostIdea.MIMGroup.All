using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.Organizations.Dto
{
    public class FindOrganizationUnitUsersInput : PagedAndFilteredInputDto
    {
        public long OrganizationUnitId { get; set; }
    }
}
