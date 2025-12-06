using Microsoft.AspNetCore.Mvc;

namespace ReservaFront.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("usuarioId") == null)
                return RedirectToAction("Login", "Auth");

            return View();
        }
    }
}
