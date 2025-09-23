using System.Collections.Generic; // Adicionado para ICollection

namespace ReservaApi.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Telefone { get; set; }
        public string? Email { get; set; }

        // A linha abaixo cria a relação "um cliente para muitas reservas".
        // Se você não precisar disso agora, pode remover ou deixar comentada.
        public ICollection<Reserva>? Reservas { get; set; }
    }
}