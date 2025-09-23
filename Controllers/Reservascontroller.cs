using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaApi.Models;
using ReservaApi.Data;

namespace ReservaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Rota base: /api/reservas
    public class ReservasController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public ReservasController(RestauranteContext context)
        {
            _context = context;
        }

        // GET: api/reservas
        // Lista todas as reservas, incluindo os dados do cliente e da mesa
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
        {
            return await _context.Reservas
                .Include(r => r.Cliente) // Inclui dados do Cliente relacionado
                .Include(r => r.Mesa)    // Inclui dados da Mesa relacionada
                .ToListAsync();
        }

        // GET: api/reservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Mesa)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
            {
                return NotFound();
            }

            return reserva;
        }
        
        // POST: api/reservas
        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva(Reserva reserva)
        {
            // --- VALIDAÇÕES (Regras de Negócio) ---

            // 1. Verifica se o cliente existe
            var cliente = await _context.Clientes.FindAsync(reserva.ClienteId);
            if (cliente == null)
            {
                return BadRequest("Cliente não encontrado.");
            }

            // 2. Verifica se a mesa existe
            var mesa = await _context.Mesas.FindAsync(reserva.MesaId);
            if (mesa == null)
            {
                return BadRequest("Mesa não encontrada.");
            }

            // 3. Verifica se a capacidade da mesa é suficiente
            if (mesa.Capacidade < reserva.Pessoas)
            {
                return BadRequest($"A mesa {mesa.Numero} suporta apenas {mesa.Capacidade} pessoas.");
            }

            // --- Fim das Validações ---

            // Adiciona a nova reserva ao contexto e salva no banco de dados
            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReserva), new { id = reserva.Id }, reserva);
        }

        // DELETE: api/reservas/5 (Cancelar a reserva)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            // Remove a reserva do contexto e salva as mudanças no banco
            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            
            return NoContent(); // Retorno padrão para DELETE bem-sucedido
        }
    }
}