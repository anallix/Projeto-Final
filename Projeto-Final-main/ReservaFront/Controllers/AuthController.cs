using Microsoft.AspNetCore.Mvc;
using ReservaFront.Services;

namespace ReservaFront.Controllers
{
    public class AuthController : Controller
    {
        private readonly ReservaApiClient _api;

        public AuthController(ReservaApiClient api)
        {
            _api = api;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string nome, string senha)
        {
            var result = await _api.LoginAsync(nome, senha);

            if (result == null)
            {
                ViewBag.Erro = "Nome ou senha inválidos.";
                return View();
            }

            HttpContext.Session.SetInt32("usuarioId", result.UsuarioId);
            HttpContext.Session.SetString("usuarioNome", result.Nome ?? "");

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
