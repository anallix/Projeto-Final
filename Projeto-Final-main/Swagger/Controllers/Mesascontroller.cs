using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaApi.Data;
using ReservaApi.Models;

namespace ReservaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MesasController : ControllerBase
    {
        private readonly RestauranteContext _context;

        public MesasController(RestauranteContext context)
        {
            _context = context;
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Mesa>>> GetMesas()
        {
            return await _context.Mesas.ToListAsync();
        }

        
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Mesa>> GetMesa(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);

            if (mesa == null)
            {
                return NotFound();
            }

            return mesa;
        }

        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Mesa>> PostMesa(Mesa mesa)
        {
            _context.Mesas.Add(mesa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMesa), new { id = mesa.Id }, mesa);
        }

       
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutMesa(int id, Mesa mesa)
        {
            if (id != mesa.Id)
            {
                return BadRequest("O ID da URL não corresponde ao ID da mesa.");
            }

            _context.Entry(mesa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Mesas.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteMesa(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa == null)
            {
                return NotFound();
            }

            _context.Mesas.Remove(mesa);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}