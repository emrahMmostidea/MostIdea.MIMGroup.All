using System.Threading.Tasks;
using MostIdea.MIMGroup.Authorization.Users;

namespace MostIdea.MIMGroup.WebHooks
{
    public interface IAppWebhookPublisher
    {
        Task PublishTestWebhook();
    }
}
