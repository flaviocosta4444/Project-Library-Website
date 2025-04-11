using Microsoft.AspNetCore.Identity;

namespace B_LEI.Models
{
    public class Requisicao
    {
        public int Id { get; set; }
        public DateTime DataRequisicao { get; set; }
        public DateTime? DataEntrega { get; set; }
        public DateTime? DataDevolucao { get; set; }

        // Ligação ao Livro
        public int LivroId { get; set; }
        public Livro? Livro { get; set; }

        // Ligação ao utilizador (Leitor) via Identity
        public string? UserId { get; set; } // ASP.NET Identity usa string para PK
        public ApplicationUser? User { get; set; }

        public bool EstadoDevolucao { get; set; }
    }

}
