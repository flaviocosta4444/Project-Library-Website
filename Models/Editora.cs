namespace B_LEI.Models
{
    public class Editora
    {
        public int EditoraId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
    

        public ICollection<Livro> Livros { get; set; } = new List<Livro>();
    }
}
