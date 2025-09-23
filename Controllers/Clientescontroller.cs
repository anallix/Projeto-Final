using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReservaApi.Models;
using ReservaApi.Data;

namespace ReservaApi.Controllers

{
    [ApiController]
    [Route("api/[controller]")] // Define a rota base como /api/clientes
    public class ClientesController : ControllerBase
    {
        private readonly RestauranteContext _context;

        // O 'contexto' do banco de dados é injetado aqui pelo ASP.NET Core
        public ClientesController(RestauranteContext context)
        {
            _context = context;
        }

        // GET: api/clientes
        // Retorna uma lista de todos os clientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            return await _context.Clientes.ToListAsync();
        }

        // GET: api/clientes/5
        // Retorna um cliente específico pelo seu Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound(); // Retorna erro 404 se não encontrar
            }

            return cliente;
        }

        // POST: api/clientes
        // Cria um novo cliente
        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();

            // Retorna um status 201 (Created) com o novo cliente e um link para ele
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
        }

        // PUT: api/clientes/5
        // Atualiza um cliente existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return BadRequest(); // Retorna erro 400 se os Ids não baterem
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Clientes.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Retorna status 204 (No Content) indicando sucesso
        }

        // DELETE: api/clientes/5
        // Deleta um cliente
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
