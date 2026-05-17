using System.Text.Json;
using System.Text.Json.Serialization;
using adosmelhoresproject.src.Models;
using adosmelhoresproject.src.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace adosmelhoresproject.src.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly string _filePath;
        public TransactionService(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.ContentRootPath, "Data", "transacao.json");

            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory!);
        }

        public List<Transaction> GetAll()
        {        
            if (!File.Exists(_filePath)) return new List<Transaction>();

            try
            {
                string jsonString = File.ReadAllText(_filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<Transaction>>(jsonString, options) ?? new List<Transaction>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler transações: {ex.Message}");
                return new List<Transaction>();
            }
        }

        private void Save(List<Transaction> lista)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(lista, options);
            File.WriteAllText(_filePath, jsonString);
        }

        public void Add(Transaction t)
        {
            var transacoes = GetAll();
            transacoes.Add(t);
            Save(transacoes);
        }

        public List<Transaction> GetByPeriod(DateTime inicio, DateTime fim)
        {
            // LINQ para filtrar antes de mandar para a UI
            return GetAll()
                .Where(t => t.Data.Date >= inicio.Date && t.Data.Date <= fim.Date)
                .OrderByDescending(t => t.Data)
                .ToList();
        }

        public decimal GetBalancePeriod(DateTime inicio, DateTime fim)
        {
            var transacoes = GetByPeriod(inicio, fim);

            // Soma tudo o que é Receita e subtrai o que é Despesa
            return transacoes.Sum(t => t.Tipo == TipoTransacao.Receita ? t.Valor : -t.Valor);
        }
    }
}
