namespace ReservaFront.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int MesaId { get; set; }
        public DateTime DataReserva { get; set; }
    }
}
