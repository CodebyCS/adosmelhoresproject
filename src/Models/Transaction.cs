namespace adosmelhoresproject.src.Models
{
    public enum TipoTransacao { Receita, Despesa }

    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public TipoTransacao Tipo { get; set; }
        public string Descricao { get; set; } = string.Empty;

        // Adiciona estas para compatibilidade com a interface que criámos
        public string Referencia { get; set; } = "N/A";
        public string Estado { get; set; } = "Concluído";
    }
}