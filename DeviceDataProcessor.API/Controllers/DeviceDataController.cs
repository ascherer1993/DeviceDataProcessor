using System.Text;
using DeviceDataProcessor.Models;
using DeviceDataProcessor.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeviceDataProcessor.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DeviceDataController : ControllerBase
{

    private readonly ILogger<DeviceDataController> _logger;
    private readonly IDeviceDataConversionService _conversionService;

    public DeviceDataController(ILogger<DeviceDataController> logger, IDeviceDataConversionService conversionService)
    {
        _logger = logger;
        _conversionService = conversionService;
    }
    
    [HttpPost("UploadJsonFiles")]
    public async Task<ActionResult> AddDeviceDataFiles([FromForm] List<IFormFile> jsonFiles)
    {
        List<string> jsonFilesAsStrings = new List<string>();
        foreach (var jsonFile in jsonFiles)
        {
            await using var stream = new MemoryStream();
            await jsonFile.CopyToAsync(stream);
            stream.Position = 0;
            using (var reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                jsonFilesAsStrings.Add(json);
            }
        }

        List<UniversalDeviceData> universalDeviceDataList = _conversionService.ConvertJsonToUnifiedDeviceData(jsonFilesAsStrings);

        return Ok(universalDeviceDataList);
    }
    
    
}