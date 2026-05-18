using Microsoft.AspNetCore.Mvc;
using adosmelhoresproject.src.Models;
using adosmelhoresproject.src.Interfaces;
using adosmelhoresproject.src.Services;
using System.Linq;

namespace adosmelhoresproject.src.Controllers
{
    public class AllocationController : Controller
    {
        private readonly IAllocationService _service;
        private readonly IEmployeeService _employeeService;
        private readonly ITransactionService _transactionService;
        private readonly DateService _dateService;

        public AllocationController(IAllocationService service, IEmployeeService employeeService, ITransactionService transactionService, DateService dateService)
        {
            _service = service;
            _employeeService = employeeService;
            _transactionService = transactionService;
            _dateService = dateService;
        }

        public IActionResult Index()
        {
            var alocacoes = _service.GetAll();

            ViewBag.Funcionarios = _employeeService.GetAll().OfType<Trainer>().Where(t => t.Active).ToList();

            return View(alocacoes);
        }

        [HttpPost]
        public IActionResult Criar(Allocation allocation)
        {
            var formador = _employeeService.GetAll().OfType<Trainer>().FirstOrDefault(t => t.Id == allocation.EmployeeId);
            if (formador != null)
            {
                allocation.EmployeeName = formador.Name;
            }
            else
            {
                allocation.EmployeeName = "Não Atribuído";
            }

            _service.Add(allocation);

            if (allocation.ValorReceita > 0)
            {
                var dataAtual = _dateService.GetCurrentDate();

                _transactionService.Add(new Transaction
                {
                    Date = dataAtual,
                    Valor = allocation.ValorReceita,
                    Type = TransactionType.Income,
                    Description = $"Faturação Formação: {allocation.NomeFormacao}",
                    Reference = $"Alocação #{allocation.Id}",
                    Status = "Recebido"
                });
            }

            return RedirectToAction("Index");
        }
    }
}