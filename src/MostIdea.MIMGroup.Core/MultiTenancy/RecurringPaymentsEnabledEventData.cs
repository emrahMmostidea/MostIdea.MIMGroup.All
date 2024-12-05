using Abp.Events.Bus;

namespace MostIdea.MIMGroup.MultiTenancy
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}