namespace adosmelhoresproject.src.Models
{
    public class Director : Employee
    {
        public bool HoursExemption { get; set; }
        public int MonthlyBonus { get; set; } = 500;
        public bool CompanyCar { get; set; }

        public override decimal CalculatePayment(DateTime startDate, DateTime endDate)
        {
            decimal baseValue = 3000m; // Adicionado 'm' para ser decimal

            if (CompanyCar)
            {
                // MonthlyBonus é int, mas ao somar com decimais (3000m e 500m), 
                // o C# converte o int automaticamente para decimal.
                this.Salary = baseValue + MonthlyBonus + 500m;
            }
            else
            {
                this.Salary = baseValue + MonthlyBonus;
            }

            return this.Salary;
        }
    }
}