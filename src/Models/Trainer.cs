namespace adosmelhoresproject.src.Models
{
    public class Trainer : Employee
    {
        public string TeachingArea { get; set; }
        public string Availability { get; set; }
        public decimal HourlyRate { get; set; }

        public override void CalculateTrainerPayment(DateTime startDate, DateTime endDate)
        {
            int Days = (endDate - startDate).Days + 1;

            this.Salary = Days * 6 * HourlyRate;
        }
    }
}