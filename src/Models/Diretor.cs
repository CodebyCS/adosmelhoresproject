namespace adosmelhoresproject.src.Models
{
    public class Diretor : Employee
    {
        public bool IsencaoHorario { get; set; }
        public int BonusMensal { get; set; }
        public bool CarroEmpresa { get; set; }
    }
}