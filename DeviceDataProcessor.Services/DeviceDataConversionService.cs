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
    private IDeviceDataRepository _dataRepository;
    private ILogger<DeviceDataConversionService> _logger;
    public DeviceDataConversionService(IDeviceDataRepository dataRepository, ILogger<DeviceDataConversionService> logger)
    {
        _dataRepository = dataRepository;
        _logger = logger;
    }
    
    public List<UniversalDeviceData> ConvertJsonToUnifiedDeviceData(params string[] jsonFiles)
    {
        return ConvertJsonToUnifiedDeviceData(jsonFiles.ToList());
    }

    public List<UniversalDeviceData> ConvertJsonToUnifiedDeviceData(List<string> jsonFiles)
    {
        if (jsonFiles.Count == 0)
        {
            _logger.LogWarning("No json strings were received. Not creating a file and returning.");
            return new List<UniversalDeviceData>();
        }

        List<UniversalDeviceData> returnList = new List<UniversalDeviceData>();
        List<UniversalDeviceData> newUniversalDeviceData = new List<UniversalDeviceData>();
        
        
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
                    _logger.LogWarning("Not a recognized schema, skipping file...");
                    continue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        returnList = _dataRepository.AddDeviceDataRangeAndSave(newUniversalDeviceData);

        return returnList;
    }

    /// <summary>
    /// Check if the property exists to determine schema of json file
    /// In a real application, you would want this to be much more robust
    /// Ideally, it would be nice if all schemas had a shared top level property that identified the schema
    /// </summary>
    /// <param name="jObjectIn"> This is a jObject that should line up with one of the accepted schemas </param>
    /// <returns> Enum value correlating with the schema type</returns>
    private DtoSchemaType GetSchemaTypeFromJObject(JObject jObjectIn)
    {
        if (jObjectIn.ContainsKey(ModelConstants.FOO1_COMPANY_ID))
        {
            return DtoSchemaType.Foo1;
        }

        if (jObjectIn.ContainsKey(ModelConstants.FOO2_COMPANY_ID))
        {
            return DtoSchemaType.Foo2;
        }

        return DtoSchemaType.Unknown;
    }

}