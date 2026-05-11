namespace adosmelhoresproject.src.Interfaces
{
    public interface IFuncionarioService
    {
        List<Funcionario> GetAll();

        void Adicionar(Funcionario f);

        void AlterarRegistoCriminal(int id, bool atualizado);

        List<Funcionarios> GetContratosValidos(DateTime dataAtual);

        List<Funcionarios> GetRegistoCriminalExpirado(DateTime dataAtual);

        decimal CalcularPagamentoFormador(int id, DateTime inicio, DateTime fim);
    }
}

