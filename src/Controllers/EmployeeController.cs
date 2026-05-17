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
        public IActionResult Index()
        {
            var employees = _service.GetAll();
            var today = _dateService.GetCurrentDate();

            ViewBag.CurrentDate = today;

            // Dados reais calculados a partir do JSON
            ViewBag.ExpiredContracts = employees.Count(e => e.ContractEndDate < today);
            ViewBag.ExpiredCriminalRecords = employees.Count(e => e.CriminalRecordDate.AddYears(1) < today);

            decimal totalSalarial = 0;
            foreach (var e in employees.Where(emp => emp.Active))
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
        public IActionResult Criar(string name, string type, decimal salary, bool HoursExemption, int MonthlyBonus, string TeachingArea, decimal HourlyRate, string Area, string DirectorName)
        {
            Employee newEmployee;

            // Tratamento flexível de strings maiúsculas/minúsculas ou EN/PT
            switch (type?.ToLower())
            {
                case "director":
                case "diretor":
                    newEmployee = new Director
                    {
                        Name = name,
                        Salary = salary,
                        HoursExemption = HoursExemption,
                        MonthlyBonus = MonthlyBonus,
                        ContractEndDate = _dateService.GetCurrentDate().AddYears(1),
                        CriminalRecordDate = _dateService.GetCurrentDate(),
                        Active = true
                    };
                    break;

                case "trainer":
                case "formador":
                    newEmployee = new Trainer
                    {
                        Name = name,
                        Salary = salary,
                        HourlyRate = HourlyRate,
                        TeachingArea = TeachingArea,
                        ContractEndDate = _dateService.GetCurrentDate().AddMonths(6),
                        CriminalRecordDate = _dateService.GetCurrentDate(),
                        Active = true
                    };
                    break;

                case "coordinator":
                case "coordenador":
                    newEmployee = new Coordinator
                    {
                        Name = name,
                        Salary = salary,
                        ContractEndDate = _dateService.GetCurrentDate().AddYears(1),
                        CriminalRecordDate = _dateService.GetCurrentDate(),
                        Active = true
                    };
                    break;

                case "secretaria":
                default:
                    newEmployee = new Secretary
                    {
                        Name = name,
                        Salary = salary,
                        Area = Area,
                        DirectorName = DirectorName,
                        ContractEndDate = _dateService.GetCurrentDate().AddYears(2),
                        CriminalRecordDate = _dateService.GetCurrentDate(),
                        Active = true
                    };
                    break;
            }

            // O Service agora calcula e atribui o ID antes de guardar no JSON
            _service.Add(newEmployee);
            return RedirectToAction("Index");
        }

        // POST: /Employee/Editar
        [HttpPost]
        public IActionResult Editar(int id, string name, string type, bool HoursExemption, int MonthlyBonus, string TeachingArea, decimal HourlyRate, string Area, string DirectorName, DateTime? CriminalRecordDate)
        {
            var funcionarios = _service.GetAll();
            var funcionarioExistente = funcionarios.FirstOrDefault(f => f.Id == id);

            if (funcionarioExistente == null) return NotFound();

            // Atualiza os dados universais
            funcionarioExistente.Name = name;

            if (CriminalRecordDate.HasValue)
            {
                funcionarioExistente.CriminalRecordDate = CriminalRecordDate.Value;
            }

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

            // SOLUÇÃO DO ERRO DO MODAL: Grava as modificações de volta no ficheiro JSON!
            // Nota: Se a tua interface IEmployeeService ainda não declarar o método "Atualizar",
            // podes fazer o cast direto para o serviço, ou adicioná-lo à tua Interface.
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