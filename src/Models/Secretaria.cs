using adosmelhoresproject.src.Models;

public class Secretaria : Employee
{
    public string DirectorName { get; set; }
    public string Area { get; set; }

    public override decimal CalculatePayment(DateTime startDate, DateTime endDate) { return 0; }
}