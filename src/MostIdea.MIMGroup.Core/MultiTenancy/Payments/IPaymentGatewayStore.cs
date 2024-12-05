using System.Collections.Generic;

namespace MostIdea.MIMGroup.MultiTenancy.Payments
{
    public interface IPaymentGatewayStore
    {
        List<PaymentGatewayModel> GetActiveGateways();
    }
}
