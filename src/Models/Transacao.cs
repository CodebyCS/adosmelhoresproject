namespace adosmelhoresproject.src.Models;

public enum TipoTransacao { Receita, Despesa }

public class Transacao
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime Data { get; set; }
    public decimal Valor { get; set; }
    public TipoTransacao Tipo { get; set; }
    public string Descricao { get; set; } = string.Empty;
}