using adosmelhoresproject.src.Models;

public class Empresa
{
    public List<Employee> Funcionarios { get; set; }= new List<Employee>();

    public decimal CalcularDespesaMensal()
    {
        decimal despesaTotal = 0;
        foreach (var funcionario in Funcionarios)
        {
            despesaTotal += funcionario.Salario;
        }

        return despesaTotal;
    }
}