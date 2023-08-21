using Newtonsoft.Json;

namespace DeviceDataProcessor.Models;

public class Foo1Dto
{
    [JsonProperty(PropertyName = "PartnerId")]
    public int PartnerId { get; set; }
    
    [JsonProperty(PropertyName = "PartnerName")]
    public string PartnerName { get; set; }
    
    [JsonProperty(PropertyName = "Trackers")]
    public List<Foo1Tracker> Trackers { get; set; }
}

public class Foo1Tracker
{
    [JsonProperty(PropertyName = "Id")]
    public int Id { get; set; }
    
    [JsonProperty(PropertyName = "Model")]
    public string Model { get; set; }
    
    [JsonProperty(PropertyName = "ShipmentStartDtm")]
    public DateTime ShipmentStartDtm { get; set; }
    
    [JsonProperty(PropertyName = "Sensors")]
    public List<Foo1Sensor> Sensors { get; set; }
}

public class Foo1Sensor
{
    [JsonProperty(PropertyName = "Id")]
    public int Id { get; set; }
    
    [JsonProperty(PropertyName = "Name")]
    public string Name { get; set; }
    
    [JsonProperty(PropertyName = "Crumbs")]
    public List<Foo1Crumb> Crumbs { get; set; }
}

public class Foo1Crumb
{
    [JsonProperty(PropertyName = "CreatedDtm")]
    public DateTime CreatedDtm { get; set; }
    
    [JsonProperty(PropertyName = "Value")]
    public double Value { get; set; }
}