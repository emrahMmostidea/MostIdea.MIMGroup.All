using System.Threading.Tasks;
using MostIdea.MIMGroup.Security.Recaptcha;

namespace MostIdea.MIMGroup.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
