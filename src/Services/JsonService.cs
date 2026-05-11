using System.Text.Json;
using System.Text.Json.Serialization;

namespace adosmelhoresproject.src.Services
{
    public class JsonService
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options;

        public JsonService(IWebHostEnvironment webHostEnvironment)
        {
            string dataFolder = Path.Combine(webHostEnvironment.WebRootPath, "Data");
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            _filePath = Path.Combine(dataFolder, "funcionarios.json");

            _options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true,
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
        }

        public async Task Guardar<T>(List<T> lista)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(lista, _options);
                await File.WriteAllTextAsync(_filePath, jsonString);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao Guardar JSON: {ex.Message}");
            }
        }

        public async Task<List<T>> Carregar<T>()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    return new List<T>();
                }
                string jsonString = await File.ReadAllTextAsync(_filePath);
                return JsonSerializer.Deserialize<List<T>>(jsonString, _options) ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao Carregar JSON: {ex.Message}");
                return new List<T>();
            }
        }
    }
}
