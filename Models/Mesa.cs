using System.Collections.Generic;

namespace ReservaApi.Models
{
    public class Mesa
    {
        public int Id { get; set; }
        public int Numero { get; set; }
        public int Capacidade { get; set; }

        // Relação: Uma mesa pode ter várias reservas.
        // Pode remover se não precisar da relação agora.
        public ICollection<Reserva>? Reservas { get; set; }
    }
}