namespace ReservaApi.Models
{
    public class Notificacao
    {
        public int Id { get; set; }
        public string? Mensagem { get; set; }
        public DateTime DataEnvio { get; set; }
    }
}