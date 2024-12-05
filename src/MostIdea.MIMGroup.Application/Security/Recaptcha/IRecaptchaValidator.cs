using System.Threading.Tasks;

namespace MostIdea.MIMGroup.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}