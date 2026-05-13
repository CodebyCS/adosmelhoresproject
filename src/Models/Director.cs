namespace adosmelhoresproject.src.Models
{
    public class Director : Employee
    {
        public bool HoursExemption { get; set; }
        public int MonthlyBonus { get; set; }
        public bool CompanyCar { get; set; }

        public override void CalculatePayment(DateTime startDate, DateTime endDate)
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
        }
    }
}