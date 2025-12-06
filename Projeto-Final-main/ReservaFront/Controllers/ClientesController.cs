using Microsoft.AspNetCore.Mvc;
using ReservaFront.Models;
using ReservaFront.Services;

namespace ReservaFront.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ReservaApiClient _api;

        public ClientesController(ReservaApiClient api)
        {
            _api = api;
        }

        // GET: Tela de cadastro
        public IActionResult Cadastro()
        {
            return View();
        }

        // POST: Envia os dados para a API
        [HttpPost]
        public async Task<IActionResult> Cadastro(Cliente cliente)
        {
            bool sucesso = await _api.CriarCliente(cliente);

            if (!sucesso)
            {
                ViewBag.Erro = "Erro ao cadastrar. Verifique os dados.";
                return View(cliente);
            }

            return RedirectToAction("Login"); 
        }
    }
}
