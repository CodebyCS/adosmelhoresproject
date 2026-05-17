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
            decimal bonus = CompanyCar ? MonthlyBonus + 500m : MonthlyBonus; // Bônus adicional se tiver carro da empresa
            int daysWorked = (endDate - startDate).Days + 1;
            decimal dailyRate = (baseValue + bonus) / 30; 
            return dailyRate * daysWorked;
        }
    }
}