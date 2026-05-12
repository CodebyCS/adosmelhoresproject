using adosmelhoresproject.src.Models;

namespace adosmelhoresproject.src.Interfaces
{
    public interface IEmployeeService
    {
        List<Employee> GetAll();

        void Adicionar(Employee f);

        void AlterarRegistoCriminal(int id, DateTime novaData);

        void AlterarContrato(int id, DateTime novaData);

        List<Employee> GetContratosValidos(DateTime dataAtual);

        List<Employee> GetRegistoCriminalExpirado(DateTime dataAtual);

        decimal CalcularPagamentoFormador(int id, DateTime inicio, DateTime fim);
    }
}

