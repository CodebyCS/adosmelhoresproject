using Microsoft.AspNetCore.Mvc;
using adosmelhoresproject.src.Interfaces;
using adosmelhoresproject.src.Services;
using adosmelhoresproject.src.Models;

namespace adosmelhoresproject.src.Controllers
{
    public class SimulatorController : Controller
    {
        private readonly DateService _dateService;
        private readonly IEmployeeService _service;
        private readonly ITransactionService _transacaoService;

        public SimulatorController(DateService dateService, IEmployeeService service, ITransactionService transacaoService)
        {
            _dateService = dateService;
            _service = service;
            _transacaoService = transacaoService;
        }

        [HttpPost]
        public IActionResult ForwardDay()
        {
            bool virouMes = _dateService.ForwardDay();
            var dataAtual = _dateService.GetCurrentDate();

            var funcionariosAtivos = _service.GetAll().Where(f => f.Active).ToList();

            if (virouMes)
            {
                ProcessPayroll(dataAtual.AddDays(-1), funcionariosAtivos);
            }

            var contratosExpirados = new List<string>();
            var registosExpirados = new List<string>();

            foreach (var func in funcionariosAtivos)
            {

                if (func.CriminalRecordDate.Date == dataAtual.Date)
                {
                    registosExpirados.Add(func.Name);
                }

                if (func.ContractEndDate.Date <= dataAtual.Date)
                {
                    if (func.ContractEndDate.Date == dataAtual.Date)
                    {
                        contratosExpirados.Add(func.Name);
                    }

                    func.Active = false;

                    if (_service is EmployeeService serviceJson)
                    {
                        serviceJson.Update(func);
                    }
                }
            }

            return Json(new
            {
                sucesso = true,
                novaData = dataAtual.ToString("yyyy-MM-dd"),
                temAlertas = contratosExpirados.Any() || registosExpirados.Any(),
                contratos = contratosExpirados,
                registos = registosExpirados
            });
        }

        // Método privado trazido do DateService para o local correto
        private void ProcessPayroll(DateTime ultimoDiaMesAnterior, List<Employee> funcionarios)
        {
            var primeiroDiaMesAnterior = new DateTime(ultimoDiaMesAnterior.Year, ultimoDiaMesAnterior.Month, 1);

            foreach (var func in funcionarios)
            {
                decimal valorAPagar = func.CalculatePayment(primeiroDiaMesAnterior, ultimoDiaMesAnterior);

                if (valorAPagar > 0)
                {
                    var pagamento = new Transaction
                    {
                        Data = ultimoDiaMesAnterior.AddDays(1), // Paga no 1º dia do novo mês
                        Valor = valorAPagar,
                        Tipo = TipoTransacao.Despesa,
                        Descricao = $"Salário: {func.Name}",
                        Referencia = func.Name,
                        Estado = "Pago"
                    };

                    _transacaoService.Add(pagamento);
                }
            }
        }
    }
}