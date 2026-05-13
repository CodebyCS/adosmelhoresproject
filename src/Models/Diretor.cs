namespace adosmelhoresproject.src.Models
{
    public class Diretor : Employee
    {
        public bool IsencaoHorario { get; set; }
        public int BonusMensal { get; set; }
        public bool CarroEmpresa { get; set; }

        public override void CalcularSalario(DateTime dataInicio, DateTime dataFim)
        {
            decimal valorBase = 3000;

            if (CarroEmpresa)
            {
                this.Salario = valorBase + BonusMensal + 500;
            }
            else
            {
                this.Salario = valorBase + BonusMensal;
            }
        }
    }
}