using Microsoft.AspNetCore.Mvc;
using adosmelhoresproject.src.Interfaces;
using adosmelhoresproject.src.Services;
using adosmelhoresproject.src.Models;
using System.Linq;
using System.Collections.Generic;
using System;

namespace adosmelhoresproject.src.Controllers
{
    public class SimulatorController : Controller
    {
        private readonly DateService _dateService;
        private readonly IEmployeeService _service;
        private readonly ITransactionService _transacaoService;
        private readonly IAllocationService _allocationService;

        public SimulatorController(DateService dateService, IEmployeeService service, ITransactionService transacaoService, IAllocationService allocationService)
        {
            _dateService = dateService;
            _service = service;
            _transacaoService = transacaoService;
            _allocationService = allocationService;
        }

        [HttpPost]
        public IActionResult ForwardDay()
        {
            bool virouMes = _dateService.ForwardDay();
            var dataAtual = _dateService.GetCurrentDate();

            var funcionariosAtivos = _service.GetAll().Where(f => f.Active).ToList();

            if (virouMes)
            {
                ProcessPayroll(dataAtual.AddDays(-1), funcionariosAtivos);
            }

            var contratosExpirados = new List<string>();
            var registosExpirados = new List<string>();

            foreach (var func in funcionariosAtivos)
            {
                if (func.CriminalRecordDate.Date == dataAtual.Date)
                {
                    registosExpirados.Add(func.Name);
                }

                if (func.ContractEndDate.Date <= dataAtual.Date)
                {
                    if (func.ContractEndDate.Date == dataAtual.Date)
                    {
                        contratosExpirados.Add(func.Name);

                        var inicioMes = new DateTime(dataAtual.Year, dataAtual.Month, 1);
                        decimal valorProporcional = func.CalculatePayment(inicioMes, dataAtual);

                        if (valorProporcional > 0)
                        {
                            _transacaoService.Add(new Transaction
                            {
                                Date = dataAtual,
                                Valor = valorProporcional,
                                Type = TransactionType.Expense,
                                Description = $"Acerto Fim de Contrato: {func.Name}",
                                Reference = func.Name,
                                Status = "Pago"
                            });
                        }
                    }

                    func.Active = false;
                    if (_service is EmployeeService serviceJson) serviceJson.Update(func);
                }
            }

            var alocacoes = _allocationService.GetAll();
            foreach (var alocacao in alocacoes)
            {
                if (alocacao.DataFim.Date == dataAtual.Date)
                {
                    var formador = _service.GetAll().OfType<Trainer>().FirstOrDefault(t => t.Id == alocacao.EmployeeId);
                    if (formador != null)
                    {
                        decimal salarioFormadorCurso = formador.CalculatePayment(alocacao.DataInicio, alocacao.DataFim, alocacao.HorasPorDia);

                        if (salarioFormadorCurso > 0)
                        {
                            _transacaoService.Add(new Transaction
                            {
                                Date = dataAtual,
                                Valor = salarioFormadorCurso,
                                Type = TransactionType.Expense,
                                Description = $"Pagamento Formador (Curso: {alocacao.NomeFormacao})",
                                Reference = formador.Name,
                                Status = "Pago"
                            });
                        }
                    }
                }
            }

            return Json(new
            {
                sucesso = true,
                novaData = dataAtual.ToString("yyyy-MM-dd"),
                temAlertas = contratosExpirados.Any() || registosExpirados.Any(),
                contratos = contratosExpirados,
                registos = registosExpirados
            });
        }

        private void ProcessPayroll(DateTime ultimoDiaMesAnterior, List<Employee> funcionarios)
        {
            var primeiroDiaMesAnterior = new DateTime(ultimoDiaMesAnterior.Year, ultimoDiaMesAnterior.Month, 1);

            foreach (var func in funcionarios)
            {
                if (func is Trainer) continue;

                decimal valorAPagar = func.CalculatePayment(primeiroDiaMesAnterior, ultimoDiaMesAnterior);

                if (valorAPagar > 0)
                {
                    var pagamento = new Transaction
                    {
                        Date = ultimoDiaMesAnterior.AddDays(1),
                        Valor = valorAPagar,
                        Type = TransactionType.Expense, 
                        Description = $"Salário: {func.Name}",
                        Reference = func.Name,
                        Status = "Pago"
                    };

                    _transacaoService.Add(pagamento);
                }
            }
        }
    }
}