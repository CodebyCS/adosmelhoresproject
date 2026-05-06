using Microsoft.AspNetCore.Mvc;

namespace adosmelhoresproject.src.Controllers
{
    public class FuncionarioController : Controller
    {
        public IActionResult Index()
        {
            //Issue: chamar _service.GetAll()
            var funcionariosFake = new List<string> { "joao", "Maria", "Pedro" };

            return View(funcionariosFake);
        }

        public IActionResult ContratosValidos()
        {
            //Issue: chamar _service.GetContratosValidos
            return View();
        }

        public IActionResult ExportarCSV()
        {
            //Issue: implementar quando service estiver pronto
            return View();
        }
    }
}
