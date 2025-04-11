using System.ComponentModel.DataAnnotations;

namespace B_LEI.Models
{
    public class AutorViewModel
    {
        [Required(ErrorMessage = "O nome do autor é obrigatório.")]
        [StringLength(100, ErrorMessage = "O {0} não pode exceder os {1} caracteres.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Select a Image File")]
        public IFormFile? Foto { get; set; }
    }
}
