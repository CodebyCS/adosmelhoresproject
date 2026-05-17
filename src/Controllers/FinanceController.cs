using Microsoft.AspNetCore.Mvc;
using adosmelhoresproject.src.Models;
using adosmelhoresproject.src.Interfaces;
using adosmelhoresproject.src.Services;

namespace adosmelhoresproject.src.Controllers
{
    public class FinanceController : Controller
    {
        private readonly ITransactionService _transactionService;
        private readonly DateService _dateService;

        public FinanceController(ITransactionService transactionService, DateService dateService)
        {
            _transactionService = transactionService;
            _dateService = dateService;
        }
        public IActionResult Index()
        {
            var hoje = _dateService.GetCurrentDate();
            var primeiroDiaMes = new DateTime(hoje.Year, hoje.Month, 1);

            var allTransactions = _transactionService.GetAll();

            // Saldo geral
            decimal balance = allTransactions.Sum(t =>
                t.Type == TransactionType.Income ? t.Valor : -t.Valor);

            // Transações do mês atual
            var monthlyTransactions = _transactionService.GetByPeriod(primeiroDiaMes, hoje);
            decimal monthlyIncome = monthlyTransactions
                .Where(t => t.Type == TransactionType.Income).Sum(t => t.Valor);
            decimal monthlyExpenses = monthlyTransactions
                .Where(t => t.Type == TransactionType.Despesa).Sum(t => t.Valor);

            decimal rentabilidade = monthlyIncome > 0
                ? ((monthlyIncome - monthlyExpenses) / monthlyIncome) * 100 : 0;

            ViewBag.SaldoAtual = balance;
            ViewBag.ReceitaMes = monthlyIncome;
            ViewBag.DespesaMes = monthlyExpenses;
            ViewBag.Rentabilidade = rentabilidade;

            // Gráfico dos últimos 4 meses
            var flow = new List<dynamic>();
            for (int i = 3; i >= 0; i--)
            {
                var mes = hoje.AddMonths(-i);
                var inicio = new DateTime(mes.Year, mes.Month, 1);
                var fim = inicio.AddMonths(1).AddDays(-1);
                var t = _transactionService.GetByPeriod(inicio, fim);
                decimal r = t.Where(x => x.Type == TransactionType.Income).Sum(x => x.Valor);
                decimal d = t.Where(x => x.Type == TransactionType.Despesa).Sum(x => x.Valor);
                decimal max = Math.Max(r, d);
                flow.Add(new
                {
                    Nome = mes.ToString("MMM"),
                    ReceitaPerc = max > 0 ? (int)(r / max * 90) : 0,
                    DespesaPerc = max > 0 ? (int)(d / max * 90) : 0,
                    Receita = r,
                    Despesa = d
                });
            }
            ViewBag.MonthlyCashFlow = flow;

            // Converte Transaction -> TransactionDTO para a view

            var dto = allTransactions.OrderByDescending(t => t.Date).Select(t => new TransactionDTO
            {
                Id = t.Id.ToString()[..8],
                Description = t.Description,
                Reference = t.Reference,
                Date = t.Date,
                Value = t.Type == TransactionType.Income ? t.Valor : -t.Valor,
                Status = t.Status
            }).ToList();

            return View(dto);

        }
    }

    // DTO simples para a View
    public class TransactionDTO
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string Reference { get; set; }
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string Status { get; set; }
    }
}