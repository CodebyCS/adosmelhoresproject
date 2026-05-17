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
            //data boas vindas
            var data = _dateService.GetCurrentDate();
            ViewBag.DataSistema = data;

            // alarme
            var funcionariosAtivos = _service.GetAll().Where(f => f.Active).ToList();

            // Conta contratos que já venceram ou vencem na data atual
            int qtdContratosExpirados = funcionariosAtivos.Count(f => f.ContractEndDate.Date <= data.Date);

            // Conta registos criminais que já venceram ou vencem na data atual
            int qtdRegistosExpirados = funcionariosAtivos.Count(f => f.CriminalRecordDate.Date <= data.Date);

            ViewBag.ContratosExpirar = qtdContratosExpirados;
            ViewBag.RegistosExpirar = qtdRegistosExpirados;

            // Deixando zerado já que as Inscrições Semanais vão ser removidas na Fase 1
            ViewBag.InscricoesPendentes = 0;
            ViewBag.NomeUtilizador = "Administrador";

            // 4. Criar a lista para o Model
            var listaAtividades = new List<ActivityViewModel>
            {
                new ActivityViewModel { Titulo = "Formação C#", Local = "Sala 1", Hora = "14:00", CorClasse = "border-primary" },
                new ActivityViewModel { Titulo = "Reunião Geral", Local = "Auditório", Hora = "16:30", CorClasse = "border-success" }
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
