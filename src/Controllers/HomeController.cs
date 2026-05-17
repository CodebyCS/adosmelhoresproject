using adosmelhoresproject.Models;
using adosmelhoresproject.src.Interfaces;
using adosmelhoresproject.src.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace adosmelhoresproject.src.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeService _service;
        private readonly DateService _dateService;

        public HomeController(ILogger<HomeController> logger, IEmployeeService service, DateService dateService)
        {
            _logger = logger;
            _service = service;
            _dateService = dateService;
        }

        public IActionResult Index()
        {
            // 1. Preencher a Data (para o topo e para as boas-vindas)
            var data = _dateService.GetCurrentDate();
            ViewBag.DataSistema = data;

            // 2. Preencher os números do gráfico
            ViewBag.InscritosPorSemana = new int[] { 8, 5, 11, 4 };

            // 3. Preencher Alertas (Simulado por agora)
            ViewBag.ContratosExpirar = 2;
            ViewBag.InscricoesPendentes = 4;
            ViewBag.NomeUtilizador = "Administrador";

            // 4. Criar a lista para o Model
            var listaAtividades = new List<AtividadeViewModel>
    {
            new AtividadeViewModel { Titulo = "Formação C#", Local = "Sala 1", Hora = "14:00", CorClasse = "border-primary" },
            new AtividadeViewModel { Titulo = "Reunião Geral", Local = "Auditório", Hora = "16:30", CorClasse = "border-success" }
    };

            return View(listaAtividades);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
