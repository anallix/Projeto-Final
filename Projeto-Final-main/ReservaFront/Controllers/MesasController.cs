using Microsoft.AspNetCore.Mvc;
using ReservaFront.Services;

namespace ReservaFront.Controllers
{
    public class MesasController : Controller
    {
        private readonly ReservaApiClient _api;

        public MesasController(ReservaApiClient api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index()
        {
            var mesas = await _api.GetMesasAsync(); 
            return View(mesas);
        }
    }
}
