using System.Text.Json;
using System.Text.Json.Serialization;
using adosmelhoresproject.src.Models;
using adosmelhoresproject.src.Interfaces;
using Microsoft.AspNetCore.Hosting; 

namespace adosmelhoresproject.src.Services;
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

    public void Adicionar(Employee f)
    {
        var funcionarios = GetAll();
        funcionarios.Add(f);
        Salvar(funcionarios);
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
            // 6 horas por dia * valor hora
            int daysWorked = (fim.Date - inicio.Date).Days + 1; // +1 para incluir o dia inicial
            if (daysWorked < 0) return 0;

            const int hoursPerDay = 6;
            return (decimal)(daysWorked * hoursPerDay * trainer.HourlyRate);
        }
        return 0;
    }
}