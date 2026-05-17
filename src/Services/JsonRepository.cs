using System.Text.Json;
using Microsoft.AspNetCore.Hosting;

namespace adosmelhoresproject.src.Services
{
    public class JsonRepository<T>
    {
        private readonly string _filePath;
        private List<T>? _cache = null;
        
        protected JsonRepository(IWebHostEnvironment env, string fileName)
        {
            _filePath = Path.Combine(env.ContentRootPath, "Data", fileName);
            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory!);
        }

        protected List<T> ReadAll()
        {
            if (_cache != null) return _cache;

            if (!File.Exists(_filePath))
            {
                _cache = new List<T>();
                return _cache;
            }

            try
            {
                var json = File.ReadAllText(_filePath);
                var option = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                _cache = JsonSerializer.Deserialize<List<T>>(json, option) ?? new List<T>();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                _cache = new List<T>();
            }
            return _cache;
        }

        protected void WriteAll(List<T> lista)
        {
            _cache = lista;
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(_filePath, JsonSerializer.Serialize(lista, options));
        }

        protected void InvalidateCache()
        {
            _cache = null;
        }
    }
}
