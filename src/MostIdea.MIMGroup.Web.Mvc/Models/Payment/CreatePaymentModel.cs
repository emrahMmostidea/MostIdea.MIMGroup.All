using MostIdea.MIMGroup.Editions;
using MostIdea.MIMGroup.MultiTenancy.Payments;

namespace MostIdea.MIMGroup.Web.Models.Payment
{
    public class CreatePaymentModel
    {
        public int EditionId { get; set; }

        public PaymentPeriodType? PaymentPeriodType { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public bool? RecurringPaymentEnabled { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }
    }
}
