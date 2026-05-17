using adosmelhoresproject.src.Models;
namespace adosmelhoresproject.src.Interfaces
{
    public interface ITransacaoService
    {
        List<Transaction> GetAll();
        void Add(Transaction t);
        List<Transaction> GetByPeriod(DateTime inicio, DateTime fim);
        decimal GetBalancePeriod(DateTime inicio, DateTime fim);
    }
}
