namespace adosmelhoresproject.src.Models
{
    public class Alocacao
    {
        public DateTime DataInicio {get; set; }
        public DateTime DataFim {get; set; }
        public string NomeFormacao {get; set; }
        public int HorasPorDia { get; set; } = 6;
    }
}
