using MostIdea.MIMGroup.MultiTenancy.Payments;

namespace MostIdea.MIMGroup.Web.Models.Payment
{
    public class CancelPaymentModel
    {
        public string PaymentId { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }
    }
}