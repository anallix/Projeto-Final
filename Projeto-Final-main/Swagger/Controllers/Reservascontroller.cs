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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reserva>>> GetReservas()
        {
            return await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Mesa)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reserva>> GetReserva(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Cliente)
                .Include(r => r.Mesa)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
                return NotFound();

            return reserva;
        }

        [HttpGet("disponibilidade")]
        public async Task<ActionResult<IEnumerable<object>>> GetDisponibilidade([FromQuery] DateTime data)
        {
            var mesas = await _context.Mesas.ToListAsync();
            var reservasDoDia = await _context.Reservas
                .Where(r => r.DataHora.Date == data.Date)
                .ToListAsync();

            var horarios = Enumerable.Range(12, 11) // 12h às 22h
                .Select(h => new TimeSpan(h, 0, 0))
                .ToList();

            var disponibilidade = new List<object>();

            foreach (var mesa in mesas)
            {
                var horariosMesa = new List<object>();

                foreach (var hora in horarios)
                {
                    bool ocupado = reservasDoDia.Any(r =>
                        r.MesaId == mesa.Id &&
                        r.DataHora.TimeOfDay == hora
                    );

                    horariosMesa.Add(new
                    {
                        horario = hora.ToString(@"hh\:mm"),
                        disponivel = !ocupado
                    });
                }

                disponibilidade.Add(new
                {
                    mesa = mesa.Numero,
                    mesaId = mesa.Id,
                    capacidade = mesa.Capacidade,
                    horarios = horariosMesa
                });
            }

            return Ok(disponibilidade);
        }

        [HttpPost]
        public async Task<ActionResult<Reserva>> PostReserva(Reserva reserva)
        {
            var cliente = await _context.Clientes.FindAsync(reserva.ClienteId);
            if (cliente == null)
                return NotFound("Cliente não encontrado.");

            var mesa = await _context.Mesas.FindAsync(reserva.MesaId);
            if (mesa == null)
                return NotFound("Mesa não encontrada.");

            if (reserva.Pessoas > mesa.Capacidade)
                return BadRequest($"A mesa {mesa.Numero} suporta apenas {mesa.Capacidade} pessoas.");

            // Verifica se já existe reserva no mesmo horário
            bool existeConflito = await _context.Reservas
                .AnyAsync(r =>
                    r.MesaId == reserva.MesaId &&
                    r.DataHora == reserva.DataHora
                );

            if (existeConflito)
                return BadRequest("Esta mesa já está reservada para esse horário.");

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReserva), new { id = reserva.Id }, reserva);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserva(int id, Reserva novaReserva)
        {
            if (id != novaReserva.Id)
                return BadRequest("ID da URL não corresponde ao ID enviado.");

            var reservaOriginal = await _context.Reservas.FindAsync(id);
            if (reservaOriginal == null)
                return NotFound("Reserva não encontrada.");

            var mesa = await _context.Mesas.FindAsync(novaReserva.MesaId);
            if (mesa == null)
                return NotFound("Mesa não encontrada.");

            if (novaReserva.Pessoas > mesa.Capacidade)
                return BadRequest($"A mesa {mesa.Numero} suporta apenas {mesa.Capacidade} pessoas.");

            // Verifica se existe outra reserva ocupando o mesmo horário
            bool conflito = await _context.Reservas.AnyAsync(r =>
                r.MesaId == novaReserva.MesaId &&
                r.DataHora == novaReserva.DataHora &&
                r.Id != id
            );

            if (conflito)
                return BadRequest("A mesa escolhida já está reservada nesse horário.");

            // Atualiza os dados
            reservaOriginal.DataHora = novaReserva.DataHora;
            reservaOriginal.MesaId = novaReserva.MesaId;
            reservaOriginal.Pessoas = novaReserva.Pessoas;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return NotFound();

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
