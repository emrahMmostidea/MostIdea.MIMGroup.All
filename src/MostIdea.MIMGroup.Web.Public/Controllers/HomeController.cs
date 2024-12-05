using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Controllers;

namespace MostIdea.MIMGroup.Web.Public.Controllers
{
    public class HomeController : MIMGroupControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}