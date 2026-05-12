using Microsoft.AspNetCore.Mvc;
using adosmelhoresproject.src.Interfaces;
using adosmelhoresproject.src.Services;

namespace adosmelhoresproject.src.Controllers
{
    public class SimuladorController : Controller
    {
        private readonly DateService _dateService;
        private readonly IFuncionarioService _service;

        public SimuladorController(DateService dateService, IFuncionarioService service)
        {
            _dateService = dateService;
            _service = service;
        }

        // GET: /Simulador
        // Mostra a data atual simulada
        public IActionResult Index()
        {
            ViewBag.DataAtual = _dateService.GetCurrentDate();
            return View();
        }

        // POST: /Simulador/AvancarDia
        // Avança um dia e verifica alertas de contratos/registos expirados
        [HttpPost]
        public IActionResult AvancarDia()
        {
            _dateService.AvancarDia();

            var dataAtual = _dateService.GetCurrentDate();
            var funcionarios = _service.GetAll();

            // Verifica funcionários cujo contrato termina hoje
            var contratosExpirados = funcionarios
                .Where(f => f.DataFimContrato.Date == dataAtual.Date)
                .ToList();

            // Verifica funcionários cujo registo criminal expira hoje
            var registosExpirados = funcionarios
                .Where(f => f.DataRegistoCriminal.Date == dataAtual.Date)
                .ToList();

            ViewBag.DataAtual = dataAtual;
            ViewBag.ContratosExpirados = contratosExpirados;
            ViewBag.RegistosExpirados = registosExpirados;
            ViewBag.TemAlertas = contratosExpirados.Any() || registosExpirados.Any();

            return View("Index");
        }
    }
}