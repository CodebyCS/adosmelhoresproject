using System.Text.Json;
using System.Text.Json.Serialization;
using adosmelhores.src.Models;
using adosmelhores.src.Interfaces;
using Microsoft.AspNetCore.Hosting; // Necessário para o IWebHostEnvironment

namespace adosmelhores.src.Services;

public class FuncionarioService : IFuncionarioService
{
    private readonly string _filePath;

    public FuncionarioService(IWebHostEnvironment env)
    {
        // CORREÇÃO: Path.Combine é estático
        _filePath = Path.Combine(env.ContentRootPath, "Data", "funcionarios.json");

        // Garante que a pasta Data existe
        var directory = Path.GetDirectoryName(_filePath);
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory!);
    }

    public List<Funcionario> GetAll()
    {
        if (!File.Exists(_filePath)) return new List<Funcionario>();

        try
        {
            string jsonString = File.ReadAllText(_filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<Funcionario>>(jsonString, options) ?? new List<Funcionario>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao ler funcionários: {ex.Message}");
            return new List<Funcionario>();
        }
    }

    // MÉTODO QUE FALTA: Responsável por escrever no arquivo
    private void Salvar(List<Funcionario> lista)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(lista, options);
        File.WriteAllText(_filePath, jsonString);
    }

    public void Adicionar(Funcionario f)
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
            funcionario.DataRegistroCriminal = novaData;
            Salvar(funcionarios);
        }
    }

    public List<Funcionario> GetContratosValidos(DateTime dataAtual)
    {
        return GetAll().Where(f => f.DataFimContrato >= dataAtual).ToList();
    }

    public List<Funcionario> GetRegistoCriminalExpirado(DateTime dataAtual)
    {
        return GetAll().Where(f => f.DataRegistroCriminal < dataAtual).ToList();
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