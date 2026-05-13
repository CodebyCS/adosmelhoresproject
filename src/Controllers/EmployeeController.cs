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

        // Injeta o service via construtor — não usa dados fake
        public EmployeeController(IEmployeeService service, DateService dateService)
        {
            _service = service;
            _dateService = dateService;
        }

        // GET: /Funcionario
        // Lista todos os funcionários
        public IActionResult Index()
        {
            var funcionarios = _service.GetAll();
            return View(funcionarios);
        }

        // GET: /Funcionario/Criar
        // Mostra o formulário para inserir novo funcionário
        public IActionResult Criar()
        {
            return View();
        }

        // POST: /Funcionario/Criar
        // Recebe os dados do formulário e guarda
        [HttpPost]
        public IActionResult Criar(Employee funcionario)
        {
            if (ModelState.IsValid)
            {
                _service.Adicionar(funcionario);
                return RedirectToAction("Index");
            }
            return View(funcionario);
        }

        // GET: /Funcionario/ContratosValidos
        // Mostra funcionários com contrato válido para a data simulada atual
        public IActionResult ContratosValidos()
        {
            var dataAtual = _dateService.GetCurrentDate();
            var funcionarios = _service.GetContratosValidos(dataAtual);
            return View(funcionarios);
        }

        // GET: /Funcionario/RegistoCriminalExpirado
        // Mostra funcionários com registo criminal expirado
        public IActionResult RegistoCriminalExpirado()
        {
            var dataAtual = _dateService.GetCurrentDate();
            var funcionarios = _service.GetRegistoCriminalExpirado(dataAtual);
            return View(funcionarios);
        }

        // GET: /Funcionario/AlterarRegistoCriminal/5
        // Mostra formulário para alterar registo criminal
        public IActionResult AlterarRegistoCriminal(int id)
        {
            var funcionario = _service.GetAll().FirstOrDefault(f => f.Id == id);
            if (funcionario == null) return NotFound();
            return View(funcionario);
        }

        // POST: /Funcionario/AlterarRegistoCriminal/5
        // Guarda a nova data do registo criminal
        [HttpPost]
        public IActionResult AlterarRegistoCriminal(int id, DateTime novaData)
        {
            _service.AlterarRegistoCriminal(id, novaData);
            return RedirectToAction("Index");
        }

        // GET: /Funcionario/CalcularPagamento/5
        // Mostra formulário para calcular pagamento de um formador
        public IActionResult CalcularPagamento(int id)
        {
            var funcionario = _service.GetAll().FirstOrDefault(f => f.Id == id);
            if (funcionario == null || funcionario is not Formador)
                return BadRequest("Funcionário não encontrado ou não é Formador.");
            return View(funcionario);
        }

        // POST: /Funcionario/CalcularPagamento/5
        // Calcula o valor a pagar ao formador com base nas datas
        [HttpPost]
        public IActionResult CalcularPagamento(int id, DateTime inicio, DateTime fim)
        {
            var total = _service.CalcularPagamentoFormador(id, inicio, fim);
            ViewBag.Total = total;
            ViewBag.Inicio = inicio;
            ViewBag.Fim = fim;
            var funcionario = _service.GetAll().FirstOrDefault(f => f.Id == id);
            return View(funcionario);
        }

        // GET: /Funcionario/ExportarCSV
        // Exporta todos os funcionários para um ficheiro CSV
        public IActionResult ExportarCSV()
        {
            var funcionarios = _service.GetAll();
            var csv = new StringBuilder();

            // Cabeçalho do CSV
            csv.AppendLine("ID,Nome,Morada,Contacto,DataFimContrato,DataRegistoCriminal,Tipo,Salario,Ativo");

            // Uma linha por funcionário
            foreach (var f in funcionarios)
            {
                csv.AppendLine($"{f.Id},{f.Name},{f.Adress},{f.Contact}," +
                               $"{f.ContractEndDate:yyyy-MM-dd},{f.CriminalRecordDate:yyyy-MM-dd}," +
                               $"{f.GetType().Name},{f.Salary},{f.Active}");
            }

            // Devolve o ficheiro para download
            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "funcionarios.csv");
        }
    }
}