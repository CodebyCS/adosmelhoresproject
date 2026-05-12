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

    public void AlterarRegistoCriminal(int id, DateTime novaData)
    {
        var funcionarios = GetAll();
        var funcionario = funcionarios.FirstOrDefault(f => f.Id == id);
        if (funcionario != null)
        {
            funcionario.DataRegistoCriminal = novaData;
            Salvar(funcionarios);
        }
    }

    public void AlterarContrato(int id, DateTime novaData)
    {
        var funcionarios = GetAll();
        var funcionario = funcionarios.FirstOrDefault(f => f.Id == id);
        if (funcionario != null)
        {
            funcionario.DataFimContrato = novaData;
            Salvar(funcionarios);
        }
    }

    public List<Employee> GetContratosValidos(DateTime dataAtual)
    {
        return GetAll().Where(f => f.DataFimContrato >= dataAtual).ToList();
    }

    public List<Employee> GetRegistoCriminalExpirado(DateTime dataAtual)
    {
        return GetAll().Where(f => f.DataRegistoCriminal < dataAtual).ToList();
    }

    public decimal CalcularPagamentoFormador(int id, DateTime inicio, DateTime fim)
    {
        var funcionario = GetAll().FirstOrDefault(f => f.Id == id);

        if (funcionario is Formador formador)
        {
            // 6 horas por dia * valor hora
            int diasTrabalhados = (fim.Date - inicio.Date).Days + 1; // +1 para incluir o dia inicial
            if (diasTrabalhados < 0) return 0;

            const int horasPorDia = 6;
            return (decimal)(diasTrabalhados * horasPorDia * formador.ValorHora);
        }
        return 0;
    }
}