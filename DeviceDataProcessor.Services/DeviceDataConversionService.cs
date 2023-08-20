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
    private static List<Type> _validFormats = new List<Type>() { typeof(Foo1Dto), typeof(Foo2Dto)};

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
        // foreach (var typeIn in _validFormats)
        // {
        //     bool isValid2 = (bool)typeof(DeviceDataConversionService)
        //         .GetMethod("IsJsonValid")
        //         .MakeGenericMethod(typeIn)
        //         .Invoke(null, new object[] { jsonFiles[0] });
        // }

        List<UniversalDeviceData> returnList = new List<UniversalDeviceData>();
        List<UniversalDeviceData> newUniversalDeviceData = new List<UniversalDeviceData>();
        
        
        foreach (var jsonContent in jsonFiles)
        {
            //For each file
            // Figure out what type it is by checking if it is valid
            // var isValid = IsJsonValid<Foo1Dto>(jsonContent);
            // var isValid2 = IsJsonValid<Foo2Dto>(jsonContent);
            //
            JObject jsonObject = JObject.Parse(jsonContent);
            // var test = Newtonsoft.Json.JsonConvert.DeserializeObject<Foo1Dto>(jsonContent);

            
            // Check if the property exists to determine schema of json file
            // Ideally, it would be nice if all schemas had a shared top level property that identified the schema
            bool isFoo1 = jsonObject.ContainsKey("PartnerId");
            bool isFoo2 = jsonObject.ContainsKey("CompanyId");
            
            if (isFoo1)
            {
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
            }
            else if (isFoo2)
            {
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
            }
            else
            {
                _logger.LogWarning("Not a recognized schema, skipping file...");
            }
        }
        
        returnList = _dataRepository.AddDeviceDataRangeAndSave(newUniversalDeviceData);

        return returnList;
    }

    // public bool ValidateJsonAgainstSchema(string json, string schema)
    // {
    //     JSchema jSchema = JSchema.Parse(schema);
    //     JToken jToken = JToken.Parse(json);
    //
    //     return jToken.IsValid(jSchema);
    // }
    //
    // public bool ValidateJsonAgainstSchema(string json, Foo1Dto schema)
    // {
    //     JSchema jSchema = JSchema.Parse(schema.ToString());
    //     JToken jToken = JToken.Parse(json);
    //
    //     return jToken.IsValid(jSchema);
    // }
    
    public static bool IsJsonValid<TSchema>(string value)
    {
        var schema1 = JSchema.Parse(value, new JSchemaReaderSettings()
        {
            Validators = new List<JsonValidator>()
            {
                new CustomDateValidator()
            }
        });
        
        
        
        
        JSchemaGenerator generator = new JSchemaGenerator();
        JSchema schema = generator.Generate(typeof(TSchema));
        // schema.Items
        // schema.Properties["ShipmentStartDtm"].Format = "MM-dd-yyyy HH:mm:ss";
        // schema.Properties["CreatedDtm"].Format = "MM-dd-yyyy HH:mm:ss";
        
        JObject jsonObject = JObject.Parse(value);
        //schema
        bool isValid = jsonObject.IsValid(schema);
        return isValid;
        
        
        
        
        
        
        // JSchemaGenerator generator = new JSchemaGenerator();
        // JSchema schema = generator.Generate(typeof(TSchema));
        // schema.AllowAdditionalProperties = false;           
        //
        // JObject obj = JObject.Parse(value);
        // return obj.IsValid(schema);
    }
    
    public class CustomDateValidator : JsonValidator
    {
        public override void Validate(JToken value, JsonValidatorContext context)
        {
            if (value.Type != JTokenType.String)
            {
                return;
            }

            var stringValue = value.ToString();
            DateTime date;
            if (!DateTime.TryParseExact(stringValue, "MM-dd-yyyy HH:mm:ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out date))
            {
                context.RaiseError($"Text '{stringValue}' is not a valid date.");
            }
        }

        public override bool CanValidate(JSchema schema) => schema.Format == "custom-date";
    }
}