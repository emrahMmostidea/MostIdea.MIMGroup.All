using System.Collections.Generic;
using MostIdea.MIMGroup.Editions;
using MostIdea.MIMGroup.Editions.Dto;
using MostIdea.MIMGroup.MultiTenancy.Payments;
using MostIdea.MIMGroup.MultiTenancy.Payments.Dto;

namespace MostIdea.MIMGroup.Web.Models.Payment
{
    public class BuyEditionViewModel
    {
        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}
