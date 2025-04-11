namespace B_LEI.Models
{
    public class Notificacao
    {
        public int Id { get; set; }

        // Título/Assunto da notificação
        public string? Titulo { get; set; }

        // Mensagem ou corpo da notificação
        public string? Mensagem { get; set; }

        // Data em que a notificação foi criada/enviada
        public DateTime DataCriacao { get; set; } = DateTime.Now;

        // Se a notificação foi lida ou não
        public bool Lida { get; set; } = false;

        // Tipo de notificação (ex.: “Info”, “Alerta”, “Erro”, etc.)
        public string? Tipo { get; set; }

        // [Opcional] Se quiseres enviar notificação sobre uma requisição ou multa
        public int? RequisicaoId { get; set; }
        public Requisicao? Requisicao { get; set; }

        public int? MultaId { get; set; }
        public Multa? Multa { get; set; }

        // FK para o utilizador a quem a notificação se dirige
        public string? UserId { get; set; }
        // Se tiveres ApplicationUser:
        // public ApplicationUser User { get; set; }
    }
}

