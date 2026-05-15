using System.Text.Json;
using System.Text.Json.Serialization;
using adosmelhoresproject.src.Models;
using adosmelhoresproject.src.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adosmelhoresproject.src.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly string _filePath;

        public EmployeeService(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.ContentRootPath, "Data", "funcionarios.json");

            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory!);
        }

        public List<Employee> GetAll()
        {
            if (!File.Exists(_filePath)) return new List<Employee>();

            try
            {
                string jsonString = File.ReadAllText(_filePath);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<Employee>>(jsonString, options) ?? new List<Employee>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao ler funcionários: {ex.Message}");
                return new List<Employee>();
            }
        }

        private void Salvar(List<Employee> lista)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(lista, options);
            File.WriteAllText(_filePath, jsonString);
        }

        // CORRIGIDO: Atribui o ID incremental correto no momento de guardar na lista real
        public void Adicionar(Employee f)
        {
            var funcionarios = GetAll();

            // Se a lista tiver elementos, apanha o maior ID e soma 1. Se não, começa no 1.
            f.Id = funcionarios.Any() ? funcionarios.Max(e => e.Id) + 1 : 1;

            funcionarios.Add(f);
            Salvar(funcionarios);
        }

        // NOVO MÉTODO CRUCIAL: Salva as edições feitas pelos Modais no ficheiro JSON
        public void Atualizar(Employee fAtualizado)
        {
            var funcionarios = GetAll();
            var index = funcionarios.FindIndex(e => e.Id == fAtualizado.Id);

            if (index != -1)
            {
                funcionarios[index] = fAtualizado; // Substitui o objeto antigo pelo editado
                Salvar(funcionarios);
            }
        }

        public void ChangeCriminalRecord(int id, DateTime newDate)
        {
            var employees = GetAll();
            var employee = employees.FirstOrDefault(f => f.Id == id);
            if (employee != null)
            {
                employee.CriminalRecordDate = newDate;
                Salvar(employees);
            }
        }

        public void AlterarContrato(int id, DateTime newDate)
        {
            var employees = GetAll();
            var employee = employees.FirstOrDefault(f => f.Id == id);
            if (employee != null)
            {
                employee.ContractEndDate = newDate;
                Salvar(employees);
            }
        }

        public List<Employee> GetValidContracts(DateTime currentDate)
        {
            return GetAll().Where(f => f.ContractEndDate >= currentDate).ToList();
        }

        public List<Employee> GetCriminalRecordExpired(DateTime currentDate)
        {
            return GetAll().Where(f => f.CriminalRecordDate < currentDate).ToList();
        }

        public decimal CalculateTrainerPayment(int id, DateTime inicio, DateTime fim)
        {
            var employee = GetAll().FirstOrDefault(f => f.Id == id);

            if (employee is Trainer trainer)
            {
                int daysWorked = (fim.Date - inicio.Date).Days + 1;
                if (daysWorked < 0) return 0;

                const int hoursPerDay = 6;
                return (decimal)(daysWorked * hoursPerDay * trainer.HourlyRate);
            }
            return 0;
        }
    }
}