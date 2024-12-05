using MostIdea.MIMGroup.Editions;
using MostIdea.MIMGroup.Editions.Dto;
using MostIdea.MIMGroup.MultiTenancy.Payments;
using MostIdea.MIMGroup.Security;
using MostIdea.MIMGroup.MultiTenancy.Payments.Dto;

namespace MostIdea.MIMGroup.Web.Models.TenantRegistration
{
    public class TenantRegisterViewModel
    {
        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public int? EditionId { get; set; }

        public SubscriptionStartType? SubscriptionStartType { get; set; }

        public EditionSelectDto Edition { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }
    }
}
