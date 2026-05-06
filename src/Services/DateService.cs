using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using adosmelhoresproject.src.Models;


namespace adosmelhoresproject.src.Services;

public class DateService
{
    private readonly string _filePath;
    private DateTime _currentDate;
    // Carrega a data simulada do arquivo JSON
    public DateService(IWebHostEnvironment env)
    {
        _filePath = Path.Combine(env.ContentRootPath, "Data", "AppState.json");

        LoadDate();
    }

    private void LoadDate()
    {
        if (!File.Exists(_filePath))
        {
            _currentDate = DateTime.Now;
            SaveDate();
            return;
        }
        try
        {
            // Leitura do arquivo JSON para obter a data simulada
            var jsonData = File.ReadAllText(_filePath);
            // Desserialização do JSON para obter a data simulada
            var appState = JsonSerializer.Deserialize<AppState>(jsonData);
            _currentDate = appState?.CurrentDate ?? DateTime.Now;
        }
        catch(Exception) 
        { 
            _currentDate = DateTime.Now;
        }

    }

    private void SaveDate()
    {
        var appState = new AppState { CurrentDate = _currentDate };
        var jsonData = JsonSerializer.Serialize(appState);
        File.WriteAllText(_filePath, jsonData);
    }

    //Funcoes publicas para uso no codigo
    public DateTime GetCurrentDate()
    {
        return _currentDate;
    }

    public void AvancarDia()
    {
        _currentDate = _currentDate.AddDays(1);
        SaveDate();

        OnDateChanged?.Invoke();
    }
    
}

