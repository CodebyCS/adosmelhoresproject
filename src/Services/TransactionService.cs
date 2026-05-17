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
                .Where(t => t.Date.Date >= inicio.Date && t.Date.Date <= fim.Date)
                .OrderByDescending(t => t.Date)
                .ToList();

        public decimal GetBalancePeriod(DateTime inicio, DateTime fim) =>
            GetByPeriod(inicio, fim)
                .Sum(t => t.Type == TransactionType.Income ? t.Valor : -t.Valor);
    }
}