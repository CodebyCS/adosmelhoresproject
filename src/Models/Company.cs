using adosmelhoresproject.src.Models;

public class Company
{
    public List<Employee> Funcionarios { get; set; }= new List<Employee>();

    public List<Transaction> Transacoes { get; set;}

    public decimal CalcularDespesaMensal()
    {
        decimal despesaTotal = 0;
        foreach (var funcionario in Funcionarios)
        {
            despesaTotal += funcionario.Salary;
        }

        return despesaTotal;
    }
}