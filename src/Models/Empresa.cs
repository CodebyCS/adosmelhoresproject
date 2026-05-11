using adosmelhoresproject.src.Models;

public class Empresa
{
    public List<Funcionario> Funcionarios { get; set; }= new List<Funcionario>();

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