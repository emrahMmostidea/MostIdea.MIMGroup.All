using System.Threading.Tasks;
using Abp.Application.Services;

namespace MostIdea.MIMGroup.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
