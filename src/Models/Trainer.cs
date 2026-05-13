namespace adosmelhoresproject.src.Models
{
    public class Trainer : Employee
    {
        public string TeachingArea { get; set; }
        public string Availability { get; set; }
        public decimal HourlyRate { get; set; }

        public decimal CalculateTrainerPayment(DateTime startDate, DateTime endDate)
        {
            int Days = (endDate - startDate).Days + 1;

            decimal totalAmount = Days * 6 * HourlyRate;
            
            return totalAmount;
        }
    }
}