namespace adosmelhoresproject.src.Models
{
    public class Director : Employee
    {
        public bool HoursExemption { get; set; }
        public int MonthlyBonus { get; set; } = 500;
        public bool CompanyCar { get; set; }

        public override decimal CalculatePayment(DateTime startDate, DateTime endDate)
        {
            decimal baseValue = 3000;

            if (CompanyCar)
            {
                this.Salary = baseValue + MonthlyBonus + 500;
            }
            else
            {            
                this.Salary = baseValue + MonthlyBonus;
            }

            return this.Salary;
        }
    }
}