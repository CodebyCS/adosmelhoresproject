using Microsoft.AspNetCore.Mvc;
using adosmelhoresproject.src.Interfaces;
using adosmelhoresproject.src.Services;
using System.Linq;

namespace adosmelhoresproject.src.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAllocationService _allocationService;
        private readonly DateService _dateService;

        public HomeController(IEmployeeService employeeService, IAllocationService allocationService, DateService dateService)
        {
            _employeeService = employeeService;
            _allocationService = allocationService;
            _dateService = dateService;
        }

        public IActionResult Index()
        {
            var today = _dateService.GetCurrentDate();
            ViewBag.DataSistema = today;


            var allEmployees = _employeeService.GetAll();

            ViewBag.ContratosExpirar = allEmployees.Count(e => e.ContractEndDate.Date < today.Date);
            ViewBag.RegistosExpirar = allEmployees.Count(e => e.CriminalRecordDate.Date < today.Date);

            var todasAlocacoes = _allocationService.GetAll();

            var cursosAtivos = todasAlocacoes
                .Where(a => a.DataInicio.Date <= today.Date && a.DataFim.Date >= today.Date)
                .OrderBy(a => a.DataInicio)
                .Take(4) 
                .ToList();

            return View(cursosAtivos); 
        }
    }
}