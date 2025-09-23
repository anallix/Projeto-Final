using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaApi.Models;
using ReservaApi.Data;

namespace ReservaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public ReservasController(RestauranteContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Busca todas as reservas cadastradas no sistema.
        /// </summary>
        /// <returns>Uma lista contendo todas as reservas.</returns>
        /// <response code="200">Retorna a lista de reservas com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
        {
            return await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Mesa)
                .ToListAsync();
        }

        /// <summary>
        /// Busca uma reserva específica pelo seu ID.
        /// </summary>
        /// <param name="id">O ID da reserva a ser buscada.</param>
        /// <returns>Os dados da reserva encontrada.</returns>
        /// <response code="200">Retorna a reserva encontrada.</response>
        /// <response code="404">Se não for encontrada uma reserva com o ID especificado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Cria uma nova reserva no sistema.
        /// </summary>
        /// <remarks>
        /// Exemplo de requisição:
        ///
        ///     POST /api/reservas
        ///     {
        ///        "dataHora": "2025-12-24T20:00:00",
        ///        "pessoas": 4,
        ///        "clienteId": 1,
        ///        "mesaId": 3
        ///     }
        ///
        /// </remarks>
        /// <param name="reserva">Objeto com os dados da nova reserva a ser criada.</param>
        /// <response code="201">Retorna a reserva recém-criada com a URL para acessá-la.</response>
        /// <response code="400">Se os dados da reserva forem inválidos (ex: a mesa não suporta a quantidade de pessoas).</response>
        /// <response code="404">Se o clienteId ou mesaId especificados não forem encontrados no banco de dados.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Reserva>> PostReserva(Reserva reserva)
        {
            var cliente = await _context.Clientes.FindAsync(reserva.ClienteId);
            if (cliente == null)
            {
                return NotFound("Cliente não encontrado.");
            }

            var mesa = await _context.Mesas.FindAsync(reserva.MesaId);
            if (mesa == null)
            {
                return NotFound("Mesa não encontrada.");
            }

            if (mesa.Capacidade < reserva.Pessoas)
            {
                return BadRequest($"A mesa {mesa.Numero} suporta apenas {mesa.Capacidade} pessoas.");
            }

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReserva), new { id = reserva.Id }, reserva);
        }

        /// <summary>
        /// Cancela uma reserva existente.
        /// </summary>
        /// <param name="id">O ID da reserva a ser cancelada.</param>
        /// <response code="204">Indica que a reserva foi cancelada com sucesso.</response>
        /// <response code="404">Se não for encontrada uma reserva com o ID especificado para cancelar.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();
            
            return NoContent();
        }
    }
}