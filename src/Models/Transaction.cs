namespace adosmelhoresproject.src.Models
{
    public enum TransactionType { Income, Expense }

    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Date { get; set; }
        public decimal Valor { get; set; }
        public TransactionType Type { get; set; }
        public string Description { get; set; } = string.Empty;

        // Adiciona estas para compatibilidade com a interface que criámos
        public string Reference { get; set; } = "N/A";
        public string Status { get; set; } = "Concluído";
    }
}