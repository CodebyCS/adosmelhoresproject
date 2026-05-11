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
                WriteIndented = true;
                PropertyNameCaseInsensitive = true;
                ReferenceHandler = ReferenceHandler.IgnoreCycles;
            }
        }


    }
}
