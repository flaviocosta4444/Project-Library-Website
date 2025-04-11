namespace B_LEI.Models
{
    public class Logs
    {
        public int Id { get; set; }

        // Data e hora do evento
        public DateTime DataEvento { get; set; } = DateTime.Now;

        // Mensagem ou descrição do que aconteceu (ex.: "Livro criado", "Login efetuado", etc.)
        public string? Mensagem { get; set; }

        // Nível do log (ex.: Info, Warn, Error)
        public string? Nivel { get; set; }

        //registar qual o utilizador que gerou a ação
        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }



    }
}
