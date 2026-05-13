namespace adosmelhoresproject.src.Models
{
    public class Formador : Employee
    {
        public string AreaLecionada { get; set; }
        public string Disponibilidade { get; set; }
        public decimal ValorHora { get; set; }

        public override void CalcularSalario(DateTime dataInicio, DateTime dataFim)
        {
            int Dias = (dataFim - dataInicio).Days + 1;

            this.Salario = Dias * 6 * ValorHora;
        }
    }
}