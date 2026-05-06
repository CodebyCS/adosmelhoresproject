namespace adosmelhoresproject.src.Interfaces
{
    public interface IFuncionario
    {
        List<Funcionario> GetAll();

        void Adicionar(IFuncionario f);

        void AlterarRegistoCriminal(int id, bool atualizado);

        List<Funcionarios> GetContratosValidos(DateTime dataAtual);

        List<Funcionarios> GetRegistoCriminalExpirado(DateTime dataAtual);

        decimal CalcularPagamentoFormador(int id, DateTime inicio, DateTime fim);
    }
}
