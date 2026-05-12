namespace adosmelhoresproject.src.Models
{
    public class Formador : Employee
    {
        public string AreaLecionada { get; set; }
        public string Disponibilidade { get; set; }
        public decimal ValorHora { get; set; }

        public decimal CalcularPagamentoFormador(DateTime dataInicio, DateTime dataFim)
        {
            int Dias = (dataFim - dataInicio).Days + 1;

            decimal valorTotal = Dias * 6 * ValorHora;
            
            return valorTotal;
        }
    }
}