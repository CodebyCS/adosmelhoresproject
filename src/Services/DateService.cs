using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using adosmelhoresproject.src.Models;

namespace adosmelhoresproject.src.Services;

public class DateService
{
    private readonly string _filePath;
    private DateTime _currentDate;
    public event Action? OnDateChanged;

    public DateService(IWebHostEnvironment env)
    {
        _filePath = Path.Combine(env.ContentRootPath, "src", "Data", "appstate.json");
        var directory = Path.GetDirectoryName(_filePath);
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory!);

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
            var jsonData = File.ReadAllText(_filePath);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var appState = JsonSerializer.Deserialize<AppState>(jsonData, options);

            if (appState == null || appState.CurrentDate.Year < 2000)
            {
                _currentDate = DateTime.Now;
            }
            else
            {
                _currentDate = appState.CurrentDate;
            }
        }
        catch
        {
            _currentDate = DateTime.Now;
        }
        SaveDate();
    }

    private void SaveDate()
    {
        var appState = new AppState { CurrentDate = _currentDate };
        var jsonData = JsonSerializer.Serialize(appState);
        File.WriteAllText(_filePath, jsonData);
    }

    public DateTime GetCurrentDate()
    {
        return _currentDate;
    }

    // migrando a gestão de data para o SimulatorController, para que ele possa processar os pagamentos e alertas
    public bool ForwardDay()
    {
        DateTime dataAnterior = _currentDate;
        _currentDate = _currentDate.AddDays(1);
        SaveDate();
        OnDateChanged?.Invoke();

        return _currentDate.Month != dataAnterior.Month;
    }
}
