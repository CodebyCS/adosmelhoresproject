using adosmelhoresproject.src.Models;
namespace adosmelhoresproject.src.Interfaces
{
    public interface ITransacaoService
    {
        List<Transacao> GetAll();
        void Adicionar(Transacao t);
        List<Transacao> GetPorPeriodo(DateTime inicio, DateTime fim);
        decimal GetSaldoPeriodo(DateTime inicio, DateTime fim);
    }
}
