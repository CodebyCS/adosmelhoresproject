using adosmelhoresproject.src.Models;
namespace adosmelhoresproject.src.Interfaces
{
    public interface IFuncionarioService
    {
        List<Funcionario> GetAll();

        void Adicionar(Funcionario f);

        void AlterarRegistoCriminal(int id, DateTime novaData);

        void AlterarContrato(int id, DateTime novaData);

        List<Funcionario> GetContratosValidos(DateTime dataAtual);

        List<Funcionario> GetRegistoCriminalExpirado(DateTime dataAtual);

        decimal CalcularPagamentoFormador(int id, DateTime inicio, DateTime fim);
    }
}

