using Microsoft.AspNetCore.Antiforgery;

namespace MostIdea.MIMGroup.Web.Controllers
{
    public class AntiForgeryController : MIMGroupControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
