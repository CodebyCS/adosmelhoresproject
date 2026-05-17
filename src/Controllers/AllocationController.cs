using Microsoft.AspNetCore.Mvc;
using adosmelhoresproject.src.Models;
using adosmelhoresproject.src.Interfaces;

namespace adosmelhoresproject.src.Controllers
{
    public class AllocationController : Controller
    {
        private readonly IAllocationService _service;
        private readonly IEmployeeService _employeeService;

        public AllocationController(IAllocationService service, IEmployeeService employeeService)
        {
            _service = service;
            _employeeService = employeeService;
        }
        public IActionResult Index()
        {
            var alocacoes = _service.GetAll();
            
            ViewBag.Funcionarios = _employeeService.GetAll().OfType<Trainer>().ToList();
            ViewBag.PercentagemLivre = 85;

            return View(alocacoes);
        }

        [HttpPost]
        public IActionResult Criar(Allocation allocation)
        {
            _service.Add(allocation);
            return RedirectToAction("Index");
        }
    }
}