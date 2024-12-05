using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Identity;

namespace MostIdea.MIMGroup.Web.Controllers
{
    public class HomeController : MIMGroupControllerBase
    {
        private readonly SignInManager _signInManager;

        public HomeController(SignInManager signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Index(string redirect = "", bool forceNewRegistration = false)
        {
            if (forceNewRegistration)
            {
                await _signInManager.SignOutAsync();
            }

            if (redirect == "TenantRegistration")
            {
                return RedirectToAction("SelectEdition", "TenantRegistration");
            }

            return AbpSession.UserId.HasValue ?
                RedirectToAction("Index", "Home", new { area = "App" }) :
                RedirectToAction("Login", "Account");
        }
    }
}