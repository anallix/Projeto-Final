using System;

namespace ReservaApi.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public int Pessoas { get; set; }

    
        public int ClienteId { get; set; }
        public int MesaId { get; set; }
        
        public Cliente? Cliente { get; set; }
        public Mesa? Mesa { get; set; }
    }
}