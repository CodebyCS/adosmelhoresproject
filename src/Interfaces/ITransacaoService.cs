using adosmelhoresproject.src.Models;
namespace adosmelhoresproject.src.Interfaces
{
    public interface ITransacaoService
    {
        List<Transacao> GetAll();
        void Adicionar(Transacao transacao);
        List<Transacao> GetPorMes(DateTime inicio, DateTime Fim);
    }
}
