namespace B_LEI.Models
{
    public class Multa
    {
        public int Id { get; set; }

        // Valor cobrado pela multa/pagamento
        public decimal Valor { get; set; }

        // Data em que a multa foi gerada
        public DateTime DataEmissao { get; set; }

        // Data do pagamento (se pago), pode ser null se ainda não foi pago
        public DateTime? DataPagamento { get; set; }

        // Motivo da multa (ex.: atraso de X dias, danos no livro, etc.)
        public string? Motivo { get; set; }

        // [Opcional] Estado da multa (pendente, paga, cancelada, etc.)
        public string? Estado { get; set; }

        // requisição / empréstimo relacionado, se for o caso
        public int? RequisicaoId { get; set; }
        public Requisicao? Requisicao { get; set; }

    

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }

}
