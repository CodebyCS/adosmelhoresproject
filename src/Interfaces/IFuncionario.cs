using adosmelhoresproject.src.Models;

namespace adosmelhoresproject.src.Interfaces
{
    public interface IFuncionario
    {
        List<Funcionario> GetAll();

        void Adicionar(Funcionario f);

        void AlterarRegistoCriminal(int id, bool atualizado);

        List<Funcionario> GetContratosValidos(DateTime dataAtual);

        List<Funcionario> GetRegistoCriminalExpirado(DateTime dataAtual);

        decimal CalcularPagamentoFormador(int id, DateTime inicio, DateTime fim);
    }
}
