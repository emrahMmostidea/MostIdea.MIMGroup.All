using MostIdea.MIMGroup.Editions.Dto;

namespace MostIdea.MIMGroup.MultiTenancy.Payments.Dto
{
    public class PaymentInfoDto
    {
        public EditionSelectDto Edition { get; set; }

        public decimal AdditionalPrice { get; set; }

        public bool IsLessThanMinimumUpgradePaymentAmount()
        {
            return AdditionalPrice < MIMGroupConsts.MinimumUpgradePaymentAmount;
        }
    }
}
