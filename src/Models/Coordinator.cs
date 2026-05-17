namespace adosmelhoresproject.src.Models
{
    public class Coordinator : Employee
    {
        public int FormadorId { get; set; }

        public List<Trainer> FormadoresAssociados { get; set; } = new List<Trainer>();

        public override decimal CalculatePayment(DateTime startDate, DateTime endDate)
        {
            // Exemplo: Retorna o salário base proporcional ou fixo
            return Salary;
        }
    }
}