using MostIdea.MIMGroup.Dto;

namespace MostIdea.MIMGroup.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
