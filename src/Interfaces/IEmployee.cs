using adosmelhoresproject.src.Models;

namespace adosmelhoresproject.src.Interfaces
{
    public interface IEmployee
    {
        List<Employee> GetAll();

        void Adicionar(Employee f);

        void AlterarRegistoCriminal(int id, bool atualizado);

        List<Employee> GetContratosValidos(DateTime dataAtual);

        List<Employee> GetRegistoCriminalExpirado(DateTime dataAtual);

        decimal CalcularPagamentoFormador(int id, DateTime inicio, DateTime fim);
    }
}
