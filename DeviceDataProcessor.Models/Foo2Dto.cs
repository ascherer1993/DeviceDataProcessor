using Newtonsoft.Json;

namespace DeviceDataProcessor.Models;

public class Foo2Dto
{
    [JsonProperty(PropertyName = "CompanyId")]
    public int CompanyId { get; set; }
    
    [JsonProperty(PropertyName = "Company")]
    public string Company { get; set; }

    [JsonProperty(PropertyName = "Devices")]
    public List<Foo2Device> Devices { get; set; } = new List<Foo2Device>();
}

public class Foo2Device
{
    [JsonProperty(PropertyName = "DeviceID")]
    public int DeviceId { get; set; }

    [JsonProperty(PropertyName = "Name")]
    public string Name { get; set; }
    
    [JsonProperty(PropertyName = "StartDateTime")]
    public DateTime StartDateTime { get; set; }

    [JsonProperty(PropertyName = "SensorData")]
    public List<Foo2SensorData> SensorData { get; set; } = new List<Foo2SensorData>();
}

public class Foo2SensorData
{
    [JsonProperty(PropertyName = "SensorType")]
    public string SensorType { get; set; }
    
    [JsonProperty(PropertyName = "DateTime")]
    public DateTime DateTime { get; set; }

    [JsonProperty(PropertyName = "Value")]
    public double Value { get; set; }
}