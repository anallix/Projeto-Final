using System;

namespace ReservaApi.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }
        public int Pessoas { get; set; }

        // Chaves estrangeiras para criar os relacionamentos
        public int ClienteId { get; set; }
        public int MesaId { get; set; }

        // Propriedades de navegação (opcional, mas recomendado para relacionamentos)
        public Cliente? Cliente { get; set; }
        public Mesa? Mesa { get; set; }
    }
}