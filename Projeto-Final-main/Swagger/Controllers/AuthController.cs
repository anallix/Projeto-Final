using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaApi.Data;
using ReservaApi.Models;

namespace ReservaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public AuthController(RestauranteContext context)
        {
            _context = context;
        }

        public class LoginRequest
        {
            public string Nome { get; set; }
            public string Senha { get; set; }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Nome) || string.IsNullOrWhiteSpace(request.Senha))
            {
                return BadRequest("Nome e senha são obrigatórios.");
            }

            var usuario = await _context.Clientes
                .FirstOrDefaultAsync(c => c.Nome == request.Nome && c.Senha == request.Senha);

            if (usuario == null)
            {
                return Unauthorized("Nome ou senha inválidos.");
            }

            return Ok(new
            {
                mensagem = "Login realizado com sucesso!",
                usuarioId = usuario.Id,
                nome = usuario.Nome
            });
        }
    }
}
