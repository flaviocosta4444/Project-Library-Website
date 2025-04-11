using Microsoft.AspNetCore.Identity;

namespace B_LEI.Models
{
    public class ApplicationUser : IdentityUser
    {
        //Motivo do bloqueio
        public string? LockoutReason { get; set; }
        // Nome 
        public string? Name { get; set; }
        // A data em que o registo do user foi criado
        public DateTime DataCriada { get; set; }

        // Morada do utilizador
        public string? Morada { get; set; }

        // Para admin confirmar o email do bibliotecario
        public bool IsEmailConfirmedByAdmin { get; set; }

        // Nome do Admin que verificou
        public string? VerifiedByAdminUsername { get; set; }
    }
}
