namespace B_LEI.Models
{
    public class Autor
    {
        public int AutorId { get; set; }
        public string? Nome { get; set; }
        public string? Foto { get; set; }

       
        public ICollection<Livro> Livros { get; set; } = new List<Livro>();
    }

}
