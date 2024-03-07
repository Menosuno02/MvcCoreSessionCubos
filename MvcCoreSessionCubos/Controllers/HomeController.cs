using Microsoft.AspNetCore.Mvc;

namespace MvcCoreSessionCubos.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
