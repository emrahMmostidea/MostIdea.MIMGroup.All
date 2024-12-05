using System.Collections.Generic;
using MostIdea.MIMGroup.Editions.Dto;
using MostIdea.MIMGroup.MultiTenancy.Payments;

namespace MostIdea.MIMGroup.Web.Models.Payment
{
    public class ExtendEditionViewModel
    {
        public EditionSelectDto Edition { get; set; }

        public List<PaymentGatewayModel> PaymentGateways { get; set; }
    }
}