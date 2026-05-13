using Microsoft.AspNetCore.Mvc;
using adosmelhoresproject.src.Interfaces;
using adosmelhoresproject.src.Services;

namespace adosmelhoresproject.src.Controllers
{
    public class SimulatorController : Controller
    {
        private readonly DateService _dateService;
        private readonly IEmployeeService _service;
        private readonly ITransacaoService _transacaoService;

        public SimulatorController(DateService dateService, IEmployeeService service, ITransacaoService transacaoService)
        {
            _dateService = dateService;
            _service = service;
            _transacaoService = transacaoService;
        }

        public IActionResult Index()
        {
            ViewBag.DataAtual = _dateService.GetCurrentDate();
            return View();
        }

        [HttpPost]
        public IActionResult AvancarDia()
        {
            _dateService.AvancarDia(_service, _transacaoService);

            var dataAtual = _dateService.GetCurrentDate();
            var funcionarios = _service.GetAll();

            var contratosExpirados = funcionarios
                .Where(f => f.ContractEndDate.Date == dataAtual.Date)
                .ToList();

            var registosExpirados = funcionarios
                .Where(f => f.ContractEndDate.Date == dataAtual.Date)
                .ToList();

            ViewBag.DataAtual = dataAtual;
            ViewBag.ContratosExpirados = contratosExpirados;
            ViewBag.RegistosExpirados = registosExpirados;
            ViewBag.TemAlertas = contratosExpirados.Any() || registosExpirados.Any();

            return View("Index");
        }
    }
}