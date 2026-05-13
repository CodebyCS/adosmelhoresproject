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

    public void AvancarDia(IEmployeeService employeeService, ITransacaoService transacaoService)
    {
        DateTime dataAnterior = _currentDate;
        _currentDate = _currentDate.AddDays(1);

        //se mudou o mes, pagamos
        if(_currentDate.Month != dataAnterior.Month)
        {
            ProcessarFolhaDePagamento(dataAnterior, employeeService, transacaoService);
        }

        SaveDate();
        OnDateChanged?.Invoke();
    }
    
    private void ProcessarFolhaDePagamento(DateTime dataReferencia, IEmployeeService employeeService, ITransacaoService transacaoService)
    {
        var employees = employeeService.GetAll().Where(f => f.Active).ToList();

        var primeiroDiaMesAnterior = new DateTime(dataReferencia.Year, dataReferencia.Month, 1);
        var ultimoDiaMesAnterior = dataReferencia.Date;

        foreach (var func in employees) 
        {
            
            decimal valorAPagar = func.CalculatePayment(ultimoDiaMesAnterior.Date, dataReferencia);

            if (valorAPagar > 0)
            {
                var pagamento = new Transacao
                {
                    Data = _currentDate,
                    Valor = valorAPagar,
                    Tipo = TipoTransacao.Despesa,
                    // 2. Usamos 'func.Name' e 'func.GetType()' em vez de 'employees'
                    Descricao = $"Salário Mensal: {func.Name} ({func.GetType().Name})"
                };

                transacaoService.Adicionar(pagamento);
            }
        }
    }
}

