using Microsoft.AspNetCore.Mvc;
using System.Text;
using adosmelhoresproject.src.Interfaces;
using adosmelhoresproject.src.Models;
using adosmelhoresproject.src.Services;

namespace adosmelhoresproject.src.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _service;
        private readonly DateService _dateService;

        public EmployeeController(IEmployeeService service, DateService dateService)
        {
            _service = service;
            _dateService = dateService;
        }

        // GET: /Employee
        public IActionResult Index()
        {
            var funcionarios = _service.GetAll();
            return View(funcionarios); // Procura por Index.cshtml
        }

        // GET: /Employee/Criar
        public IActionResult Criar()
        {
            return View("Create"); // Mapeia para Create.cshtml
        }

        [HttpPost]
        public IActionResult Criar(Employee funcionario)
        {
            if (ModelState.IsValid)
            {
                _service.Adicionar(funcionario);
                return RedirectToAction("Index");
            }
            return View("Create", funcionario);
        }

        // GET: /Employee/ContratosValidos
        public IActionResult ContratosValidos()
        {
            var dataAtual = _dateService.GetCurrentDate();
            var funcionarios = _service.GetValidContracts(dataAtual);
            // Sugestão: Criar ficheiro ValidContracts.cshtml
            return View("ValidContracts", funcionarios);
        }

        // GET: /Employee/RegistoCriminalExpirado
        public IActionResult RegistoCriminalExpirado()
        {
            var dataAtual = _dateService.GetCurrentDate();
            var funcionarios = _service.GetCriminalRecordExpired(dataAtual);
            // Sugestão: Criar ficheiro ExpiredCriminalRecord.cshtml
            return View("ExpiredCriminalRecord", funcionarios);
        }

        // GET: /Employee/AlterarRegistoCriminal/{id}
        public IActionResult AlterarRegistoCriminal(int id)
        {
            var funcionario = _service.GetAll().FirstOrDefault(f => f.Id == id);
            if (funcionario == null) return NotFound();

            // Mapeia para ChangeCriminalRegister.cshtml
            return View("ChangeCriminalRegister", funcionario);
        }

        [HttpPost]
        public IActionResult AlterarRegistoCriminal(int id, DateTime novaData)
        {
            _service.ChangeCriminalRecord(id, novaData);
            return RedirectToAction("Index");
        }

        // GET: /Employee/CalcularPagamento/{id}
        public IActionResult CalcularPagamento(int id)
        {
            var funcionario = _service.GetAll().FirstOrDefault(f => f.Id == id);

            // Verificação de segurança para garantir que é um Formador (Trainer)
            if (funcionario == null || funcionario is not Trainer)
                return BadRequest("Funcionário não encontrado ou não é um Formador.");

            // Mapeia para CalculatePayment.cshtml
            return View("CalculatePayment", funcionario);
        }

        [HttpPost]
        public IActionResult CalcularPagamento(int id, DateTime inicio, DateTime fim)
        {
            var total = _service.CalculateTrainerPayment(id, inicio, fim);

            ViewBag.Total = total;
            ViewBag.Inicio = inicio;
            ViewBag.Fim = fim;

            var funcionario = _service.GetAll().FirstOrDefault(f => f.Id == id);
            return View("CalculatePayment", funcionario);
        }

        // GET: /Employee/ExportarCSV
        public IActionResult ExportarCSV()
        {
            var funcionarios = _service.GetAll();
            var csv = new StringBuilder();

            csv.AppendLine("ID,Nome,Morada,Contacto,DataFimContrato,DataRegistoCriminal,Tipo,Salario,Ativo");

            foreach (var f in funcionarios)
            {
                csv.AppendLine($"{f.Id},{f.Name},{f.Adress},{f.Contact}," +
                               $"{f.ContractEndDate:yyyy-MM-dd},{f.CriminalRecordDate:yyyy-MM-dd}," +
                               $"{f.GetType().Name},{f.Salary},{f.Active}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "funcionarios.csv");
        }
    }
}