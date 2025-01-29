using Microsoft.AspNetCore.Mvc;

namespace portfolio.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View("Dashboard");
        }
    }
}
