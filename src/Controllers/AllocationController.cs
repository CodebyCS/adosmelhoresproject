using Microsoft.AspNetCore.Mvc;
using adosmelhoresproject.src.Models;

namespace adosmelhoresproject.src.Controllers
{
    public class AllocationController : Controller
    {
        public IActionResult Index()
        {
            // Simulação de dados (Substituir pelos teus Services futuramente)
            ViewBag.PercentagemLivre = 85;
            ViewBag.LogsAtividade = new List<string> {
                "Novo curso 'C# Avançado' registado hoje às 09:00",
                "Pagamento de receita A001 confirmado"
            };

            // Dados para o dropdown de funcionários
            ViewBag.Funcionarios = new List<Employee>();

            return View(new List<Alocacao>()); // Passa a lista de alocações
        }

        [HttpPost]
        public IActionResult Criar(Alocacao allocation)
        {
            // Lógica para salvar
            return RedirectToAction("Index");
        }
    }
}