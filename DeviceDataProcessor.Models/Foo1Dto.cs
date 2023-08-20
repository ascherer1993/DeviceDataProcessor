using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Schema;

namespace DeviceDataProcessor.Models;

public class Foo1Dto
{
    public int PartnerId { get; set; }
    
    public string PartnerName { get; set; }
    
    public List<Foo1Tracker> Trackers { get; set; }
}

public class Foo1Tracker
{
    public int Id { get; set; }
    
    public string Model { get; set; }
    
    // [DataMember]
    // [JsonConverter(typeof(DateFormatConverter), "MM-dd-yyyy HH:mm:ss")]
    // [DataType(DataType.DateTime)]
    // public DateTime ShipmentStartDtm { get; set; }
    public DateTime ShipmentStartDtm { get; set; }
    
    public List<Foo1Sensor> Sensors { get; set; }
}

public class Foo1Sensor
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    public List<Foo1Crumb> Crumbs { get; set; }
}

public class Foo1Crumb
{
    // [DataMember]
    // [JsonConverter(typeof(DateFormatConverter), "MM-dd-yyyy HH:mm:ss")]
    // [DataType(DataType.DateTime)]
    // public DateTime CreatedDtm { get; set; }
    public DateTime CreatedDtm { get; set; }
    
    public double Value { get; set; }
}


public class DateFormatConverter : IsoDateTimeConverter
{
    public DateFormatConverter(string format)
    {
        base.DateTimeFormat = format;
    }
}