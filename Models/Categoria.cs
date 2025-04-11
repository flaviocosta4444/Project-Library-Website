namespace B_LEI.Models
{
    public class Categoria
    {
        public int CategoriaId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        
  
        public ICollection<Livro> Livros { get; set; } = new List<Livro>();
    }

}
