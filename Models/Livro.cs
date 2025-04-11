namespace B_LEI.Models
{
    public class Livro
    {
        public int LivroId { get; set; }
        public string? Titulo { get; set; }
        public string? ISBN { get; set; }
        public string? Edicao { get; set; }
        public int AnoPublicacao { get; set; }
        public string? Capa { get; set; }
        public string? Descricao { get; set; }

        // Relacoes
        public int AutorId { get; set; }
        public Autor? Autor { get; set; }

        public int CategoriaId { get; set; }
        public  Categoria? Categoria { get; set; }

        public int EditoraId { get; set; }
        public  Editora? Editora { get; set; }

        public bool Estado { get; set; }
    }

}
