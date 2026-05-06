namespace adosmelhoresproject.src.Models
{
    public class Formador : Funcionario
    {
        public string AreaLecionada { get; set; }
        public string Disponibilidade { get; set; }
        public decimal ValorHora { get; set; }
    }
}