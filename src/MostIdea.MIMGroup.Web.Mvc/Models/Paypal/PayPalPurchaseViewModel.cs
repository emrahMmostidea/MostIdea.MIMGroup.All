using MostIdea.MIMGroup.MultiTenancy.Payments.Paypal;

namespace MostIdea.MIMGroup.Web.Models.Paypal
{
    public class PayPalPurchaseViewModel
    {
        public long PaymentId { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public PayPalPaymentGatewayConfiguration Configuration { get; set; }
    }
}
