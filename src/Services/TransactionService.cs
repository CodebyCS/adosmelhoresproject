using adosmelhoresproject.src.Interfaces;
using adosmelhoresproject.src.Models;
using Microsoft.AspNetCore.Hosting;

namespace adosmelhoresproject.src.Services
{
    public class TransactionService : JsonRepository<Transaction>, ITransactionService
    {
        public TransactionService(IWebHostEnvironment env)
            : base(env, "transacao.json") { }

        public List<Transaction> GetAll() => ReadAll();

        public void Add(Transaction t)
        {
            var lista = ReadAll();
            lista.Add(t);
            WriteAll(lista);
        }

        public List<Transaction> GetByPeriod(DateTime inicio, DateTime fim) =>
            ReadAll()
                .Where(t => t.Data.Date >= inicio.Date && t.Data.Date <= fim.Date)
                .OrderByDescending(t => t.Data)
                .ToList();

        public decimal GetBalancePeriod(DateTime inicio, DateTime fim) =>
            GetByPeriod(inicio, fim)
                .Sum(t => t.Tipo == TipoTransacao.Receita ? t.Valor : -t.Valor);
    }
}