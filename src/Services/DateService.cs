using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using adosmelhoresproject.src.Models;
using adosmelhoresproject.src.Interfaces;
using adosmelhoresproject.src.Services;


namespace adosmelhoresproject.src.Services;

public class DateService
{
    private readonly string _filePath;
    private DateTime _currentDate;
    // Carrega a data simulada do arquivo JSON
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

            // Se o arquivo existir mas a data for inválida (ano 0001), resetamos para HOJE
            if (appState == null || appState.CurrentDate.Year < 2000)
            {
                _currentDate = DateTime.Now;
                SaveDate();
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

    public void ForwardDay(IEmployeeService employeeService, ITransacaoService transacaoService)
    {
        DateTime dataAnterior = _currentDate;
        _currentDate = _currentDate.AddDays(1);

        //se mudou o mes, pagamos
        if(_currentDate.Month != dataAnterior.Month)
        {
            ProcessPayroll(dataAnterior, employeeService, transacaoService);
        }

        SaveDate();
        OnDateChanged?.Invoke();
    }
    
    private void ProcessPayroll(DateTime dataReferencia, IEmployeeService employeeService, ITransacaoService transacaoService)
    {
        var employees = employeeService.GetAll().Where(f => f.Active).ToList();

        var primeiroDiaMesAnterior = new DateTime(dataReferencia.Year, dataReferencia.Month, 1);
        var ultimoDiaMesAnterior = dataReferencia.Date;

        foreach (var func in employees) 
        {
            
            decimal valorAPagar = func.CalculatePayment(primeiroDiaMesAnterior, ultimoDiaMesAnterior);

            if (valorAPagar > 0)
            {
                var pagamento = new Transaction
                {
                    Data = _currentDate,
                    Valor = valorAPagar,
                    Tipo = TipoTransacao.Despesa, 
                    Descricao = $"Salário: {func.Name}",
                    Referencia = func.Name,
                    Estado = "Pago"
                };

                transacaoService.Add(pagamento);
            }
        }
    }
}

