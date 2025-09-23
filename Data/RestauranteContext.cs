using Microsoft.EntityFrameworkCore;
using ReservaApi.Models; // Usando o namespace correto 'ReservaApi'

// O namespace precisa de um par de chaves que envolve tudo
namespace ReservaApi.Data 
{
    // A classe também precisa do seu próprio par de chaves
    public class RestauranteContext : DbContext
    {
        // O construtor precisa do seu próprio par de chaves
        public RestauranteContext(DbContextOptions<RestauranteContext> options) : base(options) 
        {
            // Geralmente vazio, as chaves são obrigatórias
        }

        // As propriedades DbSet ficam dentro da classe
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Mesa> Mesas { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Notificacao> Notificacoes { get; set; }
    }
}