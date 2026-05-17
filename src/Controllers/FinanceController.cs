using Microsoft.AspNetCore.Mvc;
using adosmelhoresproject.src.Models;

namespace adosmelhoresproject.src.Controllers
{
    public class FinanceController : Controller
    {
        public IActionResult Index()
        {
            // Dados simulados para a Visão Geral
            ViewBag.SaldoAtual = 24750.50m;
            ViewBag.ReceitasMes = 8200.00m;
            ViewBag.DespesasMes = 5950.00m;

            decimal rentabilidade = ViewBag.ReceitasMes > 0
                ? ((ViewBag.ReceitasMes - ViewBag.DespesasMes) / ViewBag.ReceitasMes) * 100
                : 0;
            ViewBag.Rentabilidade = rentabilidade;

            // Dados para o Gráfico (Fluxo Mensal)
            ViewBag.FluxoMensal = new List<dynamic> {
                new { Nome="Jan", ReceitaPerc=50, DespesaPerc=30, Receita=2100, Despesa=1400 },
                new { Nome="Fev", ReceitaPerc=70, DespesaPerc=45, Receita=3200, Despesa=2100 },
                new { Nome="Mar", ReceitaPerc=90, DespesaPerc=50, Receita=4500, Despesa=2300 },
                new { Nome="Abr", ReceitaPerc=80, DespesaPerc=40, Receita=3800, Despesa=1900 }
            };

            // Lista de Transações (Normalmente viria de um serviço)
            var transacoes = new List<TransacaoDTO> {
                new TransacaoDTO { Id="F001", Descricao="Salários", Referencia="John Doe", Data=DateTime.Now, Valor=-1850m, Estado="Agendado" },
                new TransacaoDTO { Id="F003", Descricao="Inscrição", Referencia="Cliente XPTO", Data=DateTime.Now, Valor=500m, Estado="Recebido" }
            };

            return View(transacoes);
        }

        public IActionResult ExportarPDF() { /* Lógica futura */ return RedirectToAction("Index"); }
        public IActionResult ExportarExcel() { /* Lógica futura */ return RedirectToAction("Index"); }
    }

    // DTO simples para a View
    public class TransacaoDTO
    {
        public string Id { get; set; }
        public string Descricao { get; set; }
        public string Referencia { get; set; }
        public DateTime Data { get; set; }
        public decimal Valor { get; set; }
        public string Estado { get; set; }
    }
}