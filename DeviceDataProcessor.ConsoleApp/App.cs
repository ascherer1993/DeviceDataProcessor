using DeviceDataProcessor.DataAccess;
using DeviceDataProcessor.Services;
using Microsoft.Extensions.Logging;

namespace DeviceDataProcessor.ConsoleApp;

public class App
{
    //private readonly IConfiguration _configuration;
    private IDeviceDataConversionService _deviceDataConversionService;
    private readonly ILogger<App> _logger;
    
    public App(IDeviceDataConversionService deviceDataConversionService, ILogger<App> logger)
    {
        _deviceDataConversionService = deviceDataConversionService;
        _logger = logger;
    }

    public void Run(string[] args)
    {
        _logger.LogInformation("Running Application");
        
        if (args.Length < 1)
        {
            Console.WriteLine("Please include a valid path to at least one json file");
            _logger.LogWarning("Please include a valid path to at least one json file. Exiting...");
            return;
        }

        List<string> jsonFilesAsString = new List<string>();

        foreach (var filePath in args)
        {
            try
            {
                string jsonContent = File.ReadAllText(filePath);
                jsonFilesAsString.Add(jsonContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"Failed to read jsonFile with path: {filePath}");
                throw;
            }
        }

        _deviceDataConversionService.ConvertJsonToUnifiedDeviceData(jsonFilesAsString);
        _logger.LogInformation("Execution complete. Completed writing to file.");
    }
}