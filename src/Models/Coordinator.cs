namespace adosmelhoresproject.src.Models
{
    public class Coordinator : Employee
    {
        public int TrainerId { get; set; }

        public List<Trainer> AffiliatedTrainers { get; set; } = new List<Trainer>();

        public override decimal CalculatePayment(DateTime startDate, DateTime endDate)
        {
            int daysWorked = (endDate - startDate).Days + 1;
            decimal dailyRate = Salary / 30;
            return dailyRate * daysWorked;
        }
    }
}