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

        // Pagina Principal - dashboard com resumo geral
        public IActionResult Index()
        {
            return View();
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
