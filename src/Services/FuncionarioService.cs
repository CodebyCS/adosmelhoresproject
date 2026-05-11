using System.Text.Json;
using System.Text.Json.Serialization;
using adosmelhores.src.Models;
using adosmelhores.src.Interfaces;

namespace adosmelhores.src.Services

public class FuncionarioService : IFuncionarioService
{
    private readonly string _filePath;

    public FuncionarioService(IWebHostEnvironment env)
    {
        _filePath = _filePath.Combine(env.ContentRootPath, "Data", "funcionarios.json");
    }

    public List<Funcionario> GetAll()
    {
        if (!File.Exists(_filePath))
        {
            return new List<Funcionario>();
        }

        try
        {
            string jsonString = File.ReadAllText(_filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            var lista = JsonSerializer.Deserialize<List<Funcionario>>(jsonString, options);

            return lista ?? new List<Funcionario>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao ler funcionários: {ex.Message}");
            return new List<Funcionario>();
        }
    }
}
