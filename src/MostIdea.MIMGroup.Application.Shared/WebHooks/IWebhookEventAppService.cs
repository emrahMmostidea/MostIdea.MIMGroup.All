using System.Threading.Tasks;
using Abp.Webhooks;

namespace MostIdea.MIMGroup.WebHooks
{
    public interface IWebhookEventAppService
    {
        Task<WebhookEvent> Get(string id);
    }
}
