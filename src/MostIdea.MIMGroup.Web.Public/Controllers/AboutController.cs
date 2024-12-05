using Microsoft.AspNetCore.Mvc;
using MostIdea.MIMGroup.Web.Controllers;

namespace MostIdea.MIMGroup.Web.Public.Controllers
{
    public class AboutController : MIMGroupControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}