using adosmelhoresproject.src.Interfaces;
using adosmelhoresproject.src.Models;
using adosmelhoresproject.src.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text;

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
        public IActionResult Index(string filtro = null)
        {
            var today = _dateService.GetCurrentDate();
            List<Employee> employees;

            if (filtro == "contratos_validos")
            {
                employees = _service.GetValidContracts(today);
            }
            else if (filtro == "registos_expirados")
            {
                employees = _service.GetCriminalRecordExpired(today);
            }
            else
            {
                employees = _service.GetAll();
            }

            ViewBag.CurrentDate = today;
            ViewBag.FiltroAtual = filtro; 


            var allEmployees = _service.GetAll();
            ViewBag.ExpiredContracts = allEmployees.Count(e => e.ContractEndDate.Date < today.Date);
            ViewBag.ExpiredCriminalRecords = allEmployees.Count(e => e.CriminalRecordDate.Date < today.Date);

            decimal totalSalarial = 0;
            foreach (var e in allEmployees.Where(emp => emp.Active))
            {
                totalSalarial += e.Salary;
                if (e is Director diretor)
                {
                    totalSalarial += diretor.MonthlyBonus;
                }
            }
            ViewBag.TotalMonthlyExpense = totalSalarial;

            return View(employees); 
        }

        // POST: /Employee/Criar
        [HttpPost]
        public IActionResult Criar(string name, string type, decimal salary, bool HoursExemption, int MonthlyBonus, string TeachingArea, decimal HourlyRate, string Area, string DirectorName, DateTime ContractEndDate)
        {
            Employee newEmployee;
            string tipoNormalizado = type?.ToLower();

            if (tipoNormalizado == "director" || tipoNormalizado == "diretor")
            {
                newEmployee = new Director { Name = name, Salary = salary, HoursExemption = HoursExemption, MonthlyBonus = MonthlyBonus, ContractEndDate = ContractEndDate, CriminalRecordDate = _dateService.GetCurrentDate(), Active = true };
            }
            else if (tipoNormalizado == "trainer" || tipoNormalizado == "formador")
            {
                newEmployee = new Trainer { Name = name, Salary = salary, HourlyRate = HourlyRate, TeachingArea = TeachingArea, ContractEndDate = ContractEndDate, CriminalRecordDate = _dateService.GetCurrentDate(), Active = true };
            }
            else if (tipoNormalizado == "coordinator" || tipoNormalizado == "coordenador")
            {
                newEmployee = new Coordinator { Name = name, Salary = salary, ContractEndDate = ContractEndDate, CriminalRecordDate = _dateService.GetCurrentDate(), Active = true };
            }
            else
            {
                newEmployee = new Secretary { Name = name, Salary = salary, Area = Area, DirectorName = DirectorName, ContractEndDate = ContractEndDate, CriminalRecordDate = _dateService.GetCurrentDate(), Active = true };
            }

            _service.Add(newEmployee);
            return RedirectToAction("Index");
        }

        // POST: /Employee/Editar
        [HttpPost]
        public IActionResult Editar(int id, string name, string type, bool HoursExemption, int MonthlyBonus, string TeachingArea, decimal HourlyRate, string Area, string DirectorName, DateTime? CriminalRecordDate, DateTime? ContractEndDate)
        {
            var funcionarios = _service.GetAll();
            var funcionarioExistente = funcionarios.FirstOrDefault(f => f.Id == id);

            if (funcionarioExistente == null) return NotFound();

            // Atualiza os dados universais

            funcionarioExistente.Name = name;

            if (CriminalRecordDate.HasValue) funcionarioExistente.CriminalRecordDate = CriminalRecordDate.Value;
            if (ContractEndDate.HasValue) funcionarioExistente.ContractEndDate = ContractEndDate.Value; // NOVA LINHA AQUI

            // Normaliza a string do tipo para evitar erros com maiúsculas/traduções
            string tipoNormalizado = type?.ToLower();

            // Atualiza propriedades via herança polimórfica
            if (funcionarioExistente is Director d && (tipoNormalizado == "director" || tipoNormalizado == "diretor"))
            {
                d.HoursExemption = HoursExemption;
                d.MonthlyBonus = MonthlyBonus;
            }
            else if (funcionarioExistente is Trainer t && (tipoNormalizado == "trainer" || tipoNormalizado == "formador"))
            {
                t.TeachingArea = TeachingArea;
                t.HourlyRate = HourlyRate;
            }
            else if (funcionarioExistente is Secretary s && tipoNormalizado == "secretaria")
            {
                s.Area = Area;
                s.DirectorName = DirectorName;
            }

            if (_service is EmployeeService serviceJson)
            {
                serviceJson.Update(funcionarioExistente);
            }

            return RedirectToAction("Index");
        }

        // POST: /Employee/AlterarRegistoCriminal
        [HttpPost]
        public IActionResult AlterarRegistoCriminal(int id, DateTime novaData)
        {
            _service.ChangeCriminalRecord(id, novaData);
            return RedirectToAction("Index");
        }

        // GET: /Employee/ExportarCSV
        public IActionResult ExportarCSV()
        {
            var funcionarios = _service.GetAll();
            var csv = new StringBuilder();

            csv.AppendLine("ID,Nome,Tipo,Salario,Ativo,FimContrato");

            foreach (var f in funcionarios)
            {
                csv.AppendLine($"{f.Id},{f.Name},{f.GetType().Name},{f.Salary},{f.Active},{f.ContractEndDate:yyyy-MM-dd}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "funcionarios.csv");
        }
    }
}