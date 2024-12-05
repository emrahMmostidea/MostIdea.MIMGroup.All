using System.Collections.Generic;
using MostIdea.MIMGroup.Editions.Dto;
using MostIdea.MIMGroup.MultiTenancy.Payments;

namespace MostIdea.MIMGroup.Web.Models.Payment
{
    public class UpgradeEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public PaymentPeriodType PaymentPeriodType { get; set; }

        public SubscriptionPaymentType SubscriptionPaymentType { get; set; }

        public decimal? AdditionalPrice { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}