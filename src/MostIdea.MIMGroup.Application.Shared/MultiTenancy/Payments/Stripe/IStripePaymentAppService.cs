using System.Threading.Tasks;
using Abp.Application.Services;
using MostIdea.MIMGroup.MultiTenancy.Payments.Dto;
using MostIdea.MIMGroup.MultiTenancy.Payments.Stripe.Dto;

namespace MostIdea.MIMGroup.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}