using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

using ReservaApi.Data;
using ReservaApi.Models;
using ReservaApi.Controllers;

namespace ReservaApi.Tests
{
    public class TestesDeClientes
    {
        // 1. Configura o Banco Falso na Memória
        private RestauranteContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<RestauranteContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            
            var context = new RestauranteContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        // 2. TESTE: Listar Clientes
        [Fact]
        public async Task GetClientes_DeveRetornarLista()
        {
            var context = GetDatabaseContext();
            context.Clientes.Add(new Cliente { Nome = "Teste A", Email = "a@a.com", Senha="123", Telefone="11" });
            context.Clientes.Add(new Cliente { Nome = "Teste B", Email = "b@b.com", Senha="123", Telefone="22" });
            await context.SaveChangesAsync();
            
            var controller = new ClientesController(context);

            var result = await controller.GetClientes();

            IEnumerable<Cliente> lista = null;
            if (result.Value != null) 
                lista = result.Value;
            else if (result.Result is OkObjectResult ok) 
                lista = ok.Value as IEnumerable<Cliente>;

            Assert.NotNull(lista);
            Assert.Equal(2, lista.Count());
        }

        // 3. TESTE: Criar Cliente
        [Fact]
        public async Task PostCliente_DeveCriar()
        {
            var context = GetDatabaseContext();
            var controller = new ClientesController(context);
            var novo = new Cliente { Nome = "Maria", Email = "m@m.com", Senha="123", Telefone="99" };

            var result = await controller.PostCliente(novo);

            Cliente? criado = null;
            if (result.Result is CreatedAtActionResult created) 
                criado = created.Value as Cliente;
            else if (result.Result is OkObjectResult ok) 
                criado = ok.Value as Cliente;

            Assert.NotNull(criado);
            Assert.Equal("Maria", criado.Nome);
        }

        // 4. TESTE: Buscar ID Inexistente (404)
        [Fact]
        public async Task GetCliente_IdInvalido_RetornaNotFound()
        {
            var context = GetDatabaseContext();
            var controller = new ClientesController(context);

            var result = await controller.GetCliente(9999);

            Assert.True(result.Result is NotFoundResult || result.Result is NotFoundObjectResult);
        }
        
        // 5. TESTE: Buscar Cliente por ID
        [Fact]
        public async Task GetCliente_IdValido_RetornaCliente()
        {
            var context = GetDatabaseContext();
            var cliente = new Cliente { Nome = "João", Email = "j@j.com", Senha="123", Telefone="11" };
            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();
            
            var controller = new ClientesController(context);

            var result = await controller.GetCliente(cliente.Id);

            Cliente? retornado = null;
            if (result.Value != null) retornado = result.Value;
            else if (result.Result is OkObjectResult ok) retornado = ok.Value as Cliente;

            Assert.NotNull(retornado);
        }

        // 6. TESTE: Deletar
        [Fact]
        public async Task DeleteCliente_DeveApagar()
        {
            var context = GetDatabaseContext();
            var cliente = new Cliente { Nome = "Del", Email = "d@d.com", Senha="123", Telefone="00" };
            context.Clientes.Add(cliente);
            await context.SaveChangesAsync();
            
            var controller = new ClientesController(context);

            var result = await controller.DeleteCliente(cliente.Id);

            Assert.True(result is NoContentResult || result is OkObjectResult || result is OkResult);
        }
    }
}