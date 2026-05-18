using System;

namespace adosmelhoresproject.src.Models
{
    public class Trainer : Employee
    {
        public string TeachingArea { get; set; }
        public enum AvailabilityEnum { Laboral, PosLaboral, Ambas }
        public AvailabilityEnum AvailabilityTrainer { get; set; }
        public decimal HourlyRate { get; set; }

        public override decimal CalculatePayment(DateTime startDate, DateTime endDate)
        {
            int days = (endDate - startDate).Days + 1;
            if (days <= 0) return 0;

            return (Salary / 30m) * days;
        }

        public decimal CalculatePayment(DateTime startDate, DateTime endDate, int horasPorDia)
        {
            int diasUteis = 0;

            for (DateTime date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
            {
                if (date.DayOfWeek != DayOfWeek.Saturday && date.DayOfWeek != DayOfWeek.Sunday)
                {
                    diasUteis++;
                }
            }

            return diasUteis * horasPorDia * HourlyRate;
        }
    }
}