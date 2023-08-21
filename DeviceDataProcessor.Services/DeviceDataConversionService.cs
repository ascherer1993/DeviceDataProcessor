using System.Globalization;
using DeviceDataProcessor.DataAccess;
using DeviceDataProcessor.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

namespace DeviceDataProcessor.Services;

public class DeviceDataConversionService : IDeviceDataConversionService
{
    private readonly IDeviceDataRepository _dataRepository;
    private readonly ILogger<DeviceDataConversionService> _logger;
    public DeviceDataConversionService(IDeviceDataRepository dataRepository, ILogger<DeviceDataConversionService> logger)
    {
        _dataRepository = dataRepository;
        _logger = logger;
    }

    public List<UniversalDeviceData> ConvertJsonToUnifiedDeviceData(params string[] jsonFiles)
    {
        return ConvertJsonToUnifiedDeviceData(jsonFiles.ToList());
    }

    /// <summary>
    /// Receives in multiple json files as strings. These are then converted to a universal sensor class,
    /// and additional values are calculated. They are then sent to the injected data repo for storage
    /// </summary>
    /// <param name="jsonFiles">Json strings</param>
    /// <returns>List of consolidated data</returns>
    public List<UniversalDeviceData> ConvertJsonToUnifiedDeviceData(List<string> jsonFiles)
    {
        if (jsonFiles.Count == 0)
        {
            _logger.LogWarning("No json strings were received. Not creating a file and returning.");
            return new List<UniversalDeviceData>();
        }
        
        var newUniversalDeviceData = new List<UniversalDeviceData>();

        foreach (var jsonContent in jsonFiles)
        {
            JObject jsonObject;
            try
            {
                jsonObject = JObject.Parse(jsonContent);
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Failed to parse json file. Invalid json. Skipping file...");
                continue;
            }
            
            DtoSchemaType schemaType = GetSchemaTypeFromJObject(jsonObject);

            switch (schemaType)
            {
                case DtoSchemaType.Foo1:
                    Foo1Dto? foo1Dto;
                    try
                    {
                        foo1Dto = jsonObject.ToObject<Foo1Dto>();
                        if (foo1Dto == null)
                        {
                            _logger.LogWarning("Failed to parse json file. Skipping file...");
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogWarning(e, "Failed to parse json file. Skipping file...");
                        continue;
                    }

                    newUniversalDeviceData.AddRange(UniversalDeviceData.GetUniversalDeviceDataFromDto(foo1Dto));
                    break;
                case DtoSchemaType.Foo2:
                    Foo2Dto? foo2Dto;
                    try
                    {
                        foo2Dto = jsonObject.ToObject<Foo2Dto>();
                        if (foo2Dto == null)
                        {
                            _logger.LogWarning("Failed to parse json file. Skipping file...");
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogWarning(e, "Failed to parse json file. Skipping file...");
                        continue;
                    }

                    newUniversalDeviceData.AddRange(UniversalDeviceData.GetUniversalDeviceDataFromDto(foo2Dto));
                    break;
                case DtoSchemaType.Unknown:
                default:
                    _logger.LogWarning("Not a recognized schema, skipping file...");
                    continue;            }
        }
        
        List<UniversalDeviceData> returnList = _dataRepository.AddDeviceDataRangeAndSave(newUniversalDeviceData);

        return returnList;
    }

    /// <summary>
    /// Check if the property exists to determine schema of json file
    /// In a real application, you might want this to be much more robust
    /// Additionally, it would be nice if all schemas had a shared top level property that identified the schema
    /// </summary>
    /// <param name="jObjectIn"> This is a jObject that should line up with one of the accepted schemas </param>
    /// <returns> Enum value correlating with the schema type</returns>
    private DtoSchemaType GetSchemaTypeFromJObject(JObject jObjectIn)
    {
        if (jObjectIn.ContainsKey(ModelConstants.FOO1_COMPANY_ID) && jObjectIn.ContainsKey(ModelConstants.FOO1_COMPANY_NAME))
        {
            return DtoSchemaType.Foo1;
        }

        if (jObjectIn.ContainsKey(ModelConstants.FOO2_COMPANY_ID) && jObjectIn.ContainsKey(ModelConstants.FOO2_COMPANY_NAME))
        {
            return DtoSchemaType.Foo2;
        }

        return DtoSchemaType.Unknown;
    }

}