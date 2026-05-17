namespace adosmelhoresproject.src.Models
{
    public class Trainer : Employee
    {
        public string TeachingArea { get; set; }
        public string Availability { get; set; }
        public decimal HourlyRate { get; set; }

        public override decimal CalculatePayment(DateTime startDate, DateTime endDate)
        {
            int days = (endDate - startDate).Days + 1;
            if (days <= 0) return 0;
            return days * 6 * HourlyRate; // Considerando 6 horas por dia
        }
    }
}