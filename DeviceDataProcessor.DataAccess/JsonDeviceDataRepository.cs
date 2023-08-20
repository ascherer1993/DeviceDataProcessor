using DeviceDataProcessor.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DeviceDataProcessor.DataAccess;

public class JsonDeviceDataRepository : IDeviceDataRepository
{
    private readonly ILogger<JsonDeviceDataRepository> _logger;
    public JsonDeviceDataRepository(ILogger<JsonDeviceDataRepository> logger)
    {
        _logger = logger;
    }
    
    public List<UniversalDeviceData> AddDeviceDataRangeAndSave(List<UniversalDeviceData> universalDeviceDataList)
    {
        string currentPath = Directory.GetCurrentDirectory();

        // List<UniversalDeviceData> currentSavedData = new List<UniversalDeviceData>();
        //
        //
        // //Combined list
        // List<UniversalDeviceData> newDataToSave = currentSavedData.Concat(universalDeviceDataList).ToList();
        
        string json = JsonConvert.SerializeObject(universalDeviceDataList);

        string solutionFolder = AppContext.BaseDirectory;
        var directoryTest = Test.TryGetSolutionDirectoryInfo(solutionFolder);
        var directoryTest2 = directoryTest.FullName;
        
        var directoryPath = @$"{currentPath}/Output";
        
        // Create the directory if it doesn't exist
        if (!Directory.Exists(directoryPath))
        {
            _logger.LogInformation("Creating folder for json output at {DirectoryPath}", directoryPath);
            Directory.CreateDirectory(directoryPath);
        }
        
        DateTime currentDateTime = DateTime.Now;
        try
        {
            string filePath = @$"{directoryPath}/{currentDateTime:yyyy-MM-dd_hh-mm-ss}_StandardizedList.json";
            File.WriteAllText(filePath, json);
            _logger.LogInformation("Successfully wrote to {DirectoryPath}", filePath);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to write json to file");
            throw;
        }
        
        return universalDeviceDataList;
    }
}

public static class Test
{
    public static DirectoryInfo TryGetSolutionDirectoryInfo(string currentPath = null)
    {
        var directory = new DirectoryInfo(
            currentPath ?? Directory.GetCurrentDirectory());
        while (directory != null && !directory.GetDirectories("JsonOutput").Any())
        {
            directory = directory.Parent;
        }
        return directory;
    }
}