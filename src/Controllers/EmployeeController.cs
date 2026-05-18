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

            var inicioMes = new DateTime(today.Year, today.Month, 1);
            var fimMes = inicioMes.AddMonths(1).AddDays(-1);

            foreach (var e in allEmployees.Where(emp => emp.Active))
            {
                if (e.ContractEndDate.Date < inicioMes.Date)
                {
                    continue;
                }

                DateTime dataInicioCalculo = inicioMes;
                DateTime dataFimCalculo = e.ContractEndDate.Date < fimMes.Date ? e.ContractEndDate.Date : fimMes.Date;

                if (dataInicioCalculo <= dataFimCalculo)
                {
                    totalSalarial += e.CalculatePayment(dataInicioCalculo, dataFimCalculo);
                }
            }

            ViewBag.TotalMonthlyExpense = totalSalarial;

            return View(employees);
        }

        // POST: /Employee/Criar
        [HttpPost]
        public IActionResult Criar(
            string name, string type, decimal salary, DateTime ContractEndDate,
            bool HoursExemption, int MonthlyBonus, bool CompanyCar,
            string TeachingArea, decimal HourlyRate, string Availability,
            string Area, string DirectorName,
            List<int> SelectedTrainerIds)
        {
            Employee newEmployee;
            string tipoNormalizado = type?.ToLower();

            if (tipoNormalizado == "director" || tipoNormalizado == "diretor")
            {
                newEmployee = new Director
                {
                    Name = name,
                    Salary = salary,
                    ContractEndDate = ContractEndDate,
                    CriminalRecordDate = _dateService.GetCurrentDate(),
                    Active = true,
                    HoursExemption = HoursExemption,
                    MonthlyBonus = MonthlyBonus,
                    CompanyCar = CompanyCar
                };
            }
            else if (tipoNormalizado == "trainer" || tipoNormalizado == "formador")
            {
                Trainer.AvailabilityEnum disponibilidade = Trainer.AvailabilityEnum.Ambas;
                if (Availability == "Laboral") disponibilidade = Trainer.AvailabilityEnum.Laboral;
                else if (Availability == "Pós-Laboral") disponibilidade = Trainer.AvailabilityEnum.PosLaboral;

                newEmployee = new Trainer
                {
                    Name = name,
                    Salary = salary,
                    ContractEndDate = ContractEndDate,
                    CriminalRecordDate = _dateService.GetCurrentDate(),
                    Active = true,
                    TeachingArea = TeachingArea,
                    HourlyRate = HourlyRate,
                    AvailabilityTrainer = disponibilidade
                };
            }
            else if (tipoNormalizado == "coordinator" || tipoNormalizado == "coordenador")
            {
                var coordenador = new Coordinator
                {
                    Name = name,
                    Salary = salary,
                    ContractEndDate = ContractEndDate,
                    CriminalRecordDate = _dateService.GetCurrentDate(),
                    Active = true
                };

                if (SelectedTrainerIds != null && SelectedTrainerIds.Any())
                {
                    var todosFormadores = _service.GetAll().OfType<Trainer>().ToList();
                    coordenador.AffiliatedTrainers = todosFormadores.Where(t => SelectedTrainerIds.Contains(t.Id)).ToList();
                }
                newEmployee = coordenador;
            }
            else
            {
                newEmployee = new Secretary
                {
                    Name = name,
                    Salary = salary,
                    ContractEndDate = ContractEndDate,
                    CriminalRecordDate = _dateService.GetCurrentDate(),
                    Active = true,
                    Area = Area,
                    DirectorName = DirectorName
                };
            }

            _service.Add(newEmployee);
            return RedirectToAction("Index");
        }

        // POST: /Employee/Editar
        [HttpPost]
        public IActionResult Editar(
            int id, string name, string type, decimal? salary, DateTime? ContractEndDate, DateTime? CriminalRecordDate,
            bool? HoursExemption, int? MonthlyBonus, bool? CompanyCar,
            string TeachingArea, decimal? HourlyRate, string Availability,
            string Area, string DirectorName,
            List<int> SelectedTrainerIds)
        {
            var funcionarios = _service.GetAll();
            var funcionarioExistente = funcionarios.FirstOrDefault(f => f.Id == id);

            if (funcionarioExistente == null) return NotFound();

            if (!string.IsNullOrEmpty(name)) funcionarioExistente.Name = name;
            if (salary.HasValue) funcionarioExistente.Salary = salary.Value;
            if (CriminalRecordDate.HasValue) funcionarioExistente.CriminalRecordDate = CriminalRecordDate.Value;

            if (ContractEndDate.HasValue)
            {
                funcionarioExistente.ContractEndDate = ContractEndDate.Value;
                if (funcionarioExistente.ContractEndDate.Date >= _dateService.GetCurrentDate().Date)
                {
                    funcionarioExistente.Active = true;
                }
            }

            string tipoNormalizado = type?.ToLower();

            if (funcionarioExistente is Director d && (tipoNormalizado == "director" || tipoNormalizado == "diretor"))
            {
                if (HoursExemption.HasValue) d.HoursExemption = HoursExemption.Value;
                if (MonthlyBonus.HasValue) d.MonthlyBonus = MonthlyBonus.Value;
                if (CompanyCar.HasValue) d.CompanyCar = CompanyCar.Value;
            }
            else if (funcionarioExistente is Trainer t && (tipoNormalizado == "trainer" || tipoNormalizado == "formador"))
            {
                if (!string.IsNullOrEmpty(TeachingArea)) t.TeachingArea = TeachingArea;
                if (HourlyRate.HasValue) t.HourlyRate = HourlyRate.Value;

                if (Availability == "Laboral") t.AvailabilityTrainer = Trainer.AvailabilityEnum.Laboral;
                else if (Availability == "Pós-Laboral") t.AvailabilityTrainer = Trainer.AvailabilityEnum.PosLaboral;
                else if (Availability == "Ambas") t.AvailabilityTrainer = Trainer.AvailabilityEnum.Ambas;
            }
            else if (funcionarioExistente is Secretary s && tipoNormalizado == "secretaria")
            {
                if (!string.IsNullOrEmpty(Area)) s.Area = Area;
                if (!string.IsNullOrEmpty(DirectorName)) s.DirectorName = DirectorName;
            }
            else if (funcionarioExistente is Coordinator c && (tipoNormalizado == "coordinator" || tipoNormalizado == "coordenador"))
            {
                if (SelectedTrainerIds != null)
                {
                    var todosFormadores = _service.GetAll().OfType<Trainer>().ToList();
                    c.AffiliatedTrainers = todosFormadores.Where(f => SelectedTrainerIds.Contains(f.Id)).ToList();
                }
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

            csv.AppendLine("SEP=;");
            csv.AppendLine("ID;Nome;Tipo;Salario;Ativo;FimContrato");

            foreach (var f in funcionarios)
            {
                csv.AppendLine($"{f.Id};{f.Name};{f.GetType().Name};{f.Salary};{f.Active};{f.ContractEndDate:yyyy-MM-dd}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "funcionarios.csv");
        }
    }
}