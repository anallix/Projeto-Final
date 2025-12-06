using System.Collections.Generic;

namespace ReservaApi.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }


        public string? Senha { get; set; }

        public ICollection<Reserva>? Reservas { get; set; }
    }
}
