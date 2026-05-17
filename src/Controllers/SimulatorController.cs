using Microsoft.AspNetCore.Mvc;
using adosmelhoresproject.src.Interfaces;
using adosmelhoresproject.src.Services;

namespace adosmelhoresproject.src.Controllers
{
    public class SimulatorController : Controller
    {
        private readonly DateService _dateService;
        private readonly IEmployeeService _service;
        private readonly ITransacaoService _transacaoService; // Adicionado

        // No construtor, pedimos os 3 serviços
        public SimulatorController(DateService dateService, IEmployeeService service, ITransacaoService transacaoService)
        {
            _dateService = dateService;
            _service = service;
            _transacaoService = transacaoService;
        }

        [HttpPost]
        public IActionResult AvancarDia()
        {
            // 1. Executa a lógica de avançar o dia e processar pagamentos
            _dateService.ForwardDay(_service, _transacaoService);

            var dataAtual = _dateService.GetCurrentDate();
            var funcionarios = _service.GetAll();

            // 2. Calcula os alertas
            var contratosExpirados = funcionarios
                .Where(f => f.ContractEndDate.Date == dataAtual.Date)
                .Select(f => f.Name).ToList();

            var registosExpirados = funcionarios
                .Where(f => f.CriminalRecordDate.Date == dataAtual.Date)
                .Select(f => f.Name).ToList();

            // 3. Devolve um objeto JSON com os resultados
            return Json(new
            {
                sucesso = true,
                novaData = dataAtual.ToString("dd/MM/yyyy"),
                temAlertas = contratosExpirados.Any() || registosExpirados.Any(),
                contratos = contratosExpirados,
                registos = registosExpirados
            });
        }
    }
}