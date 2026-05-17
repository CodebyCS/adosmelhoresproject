using adosmelhoresproject.src.Models;

namespace adosmelhoresproject.src.Models;

public class Secretary : Employee
{
    public string DirectorName { get; set; }
    public string Area { get; set; }

    public override decimal CalculatePayment(DateTime startDate, DateTime endDate)
    {
        int daysWorked = (endDate - startDate).Days + 1;
        decimal dailyRate = Salary / 30;
        return dailyRate * daysWorked;
    }
}