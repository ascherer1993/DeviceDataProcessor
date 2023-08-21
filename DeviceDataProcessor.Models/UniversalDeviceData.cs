namespace DeviceDataProcessor.Models;

public class UniversalDeviceData
{
    public int CompanyId { get; set; } // Foo1: PartnerId, Foo2: CompanyId
    public string CompanyName { get; set; } // Foo1: PartnerName, Foo2: Company
    public int? DeviceId { get; set; } // Foo1: Id, Foo2: DeviceID
    public string DeviceName { get; set; } // Foo1: Model, Foo2: Name

    public DateTime?
        FirstReadingDtm
    {
        get;
        set;
    } // Foo1: Trackers.Sensors.Crumbs, Foo2: Devices.SensorData public DateTime? LastReadingDtm { get; set; }

    public DateTime? LastReadingDtm { get; set; } // Foo1: Trackers.Sensors.Crumbs, Foo2: Devices.SensorData

    //Calculated Values
    public int? TemperatureCount { get; set; }
    public double? AverageTemperature { get; set; }
    public int? HumidityCount { get; set; }
    public double? AverageHumidity { get; set; }


    public static List<UniversalDeviceData> GetUniversalDeviceDataFromDto(Foo1Dto dto)
    {
        var universalDeviceDataList = new List<UniversalDeviceData>();

        foreach (Foo1Tracker tracker in dto.Trackers)
        {
            var universalDeviceData = new UniversalDeviceData();
            universalDeviceData.CompanyId = dto.PartnerId;
            universalDeviceData.CompanyName = dto.PartnerName;
            universalDeviceData.DeviceId = tracker.Id;
            universalDeviceData.DeviceName = tracker.Model;
            
            //Could have an empty list of sensors
            if (!tracker.Sensors.Any())
            {
                universalDeviceData.TemperatureCount = 0;
                universalDeviceData.HumidityCount = 0;
                continue;
            }
            
            universalDeviceData.TemperatureCount =
                tracker.Sensors.FirstOrDefault(f => f.Name == ModelConstants.FOO1_TEMPERATURE)?.Crumbs.Count;
            
            universalDeviceData.HumidityCount = tracker.Sensors
                .FirstOrDefault(f => f.Name == ModelConstants.FOO1_HUMIDTY || f.Name == ModelConstants.FOO1_HUMIDITY)
                ?.Crumbs
                .Count;
            
            //Could have an empty list of crumbs
            if (!tracker.Sensors.SelectMany(f => f.Crumbs).Any())
                continue;
            
            universalDeviceData.FirstReadingDtm =
                tracker.Sensors.SelectMany(f => f.Crumbs).Min(f => f.CreatedDtm);
            universalDeviceData.LastReadingDtm =
                tracker.Sensors.SelectMany(f => f.Crumbs).DefaultIfEmpty()?.Max(f => f.CreatedDtm);

            //Calculations
            //These items only get the first instance of sensor with the name Temperature or Humidty/Humidity per device
            
            universalDeviceData.AverageTemperature = tracker.Sensors
                .FirstOrDefault(f => f.Name == ModelConstants.FOO1_TEMPERATURE)?.Crumbs
                .Average(g => g.Value);
            universalDeviceData.AverageHumidity = tracker.Sensors
                .FirstOrDefault(f => f.Name == ModelConstants.FOO1_HUMIDTY || f.Name == ModelConstants.FOO1_HUMIDITY)
                ?.Crumbs
                .Average(g => g.Value);

            universalDeviceDataList.Add(universalDeviceData);
        }

        return universalDeviceDataList;
    }

    public static List<UniversalDeviceData> GetUniversalDeviceDataFromDto(Foo2Dto dto)
    {
        var universalDeviceDataList = new List<UniversalDeviceData>();

        foreach (Foo2Device device in dto.Devices)
        {
            var universalDeviceData = new UniversalDeviceData();
            universalDeviceData.CompanyId = dto.CompanyId;
            universalDeviceData.CompanyName = dto.Company;
            universalDeviceData.DeviceId = device.DeviceId;
            universalDeviceData.DeviceName = device.Name;
            
            if (!device.SensorData.Any())
            {
                universalDeviceData.TemperatureCount = 0;
                universalDeviceData.HumidityCount = 0;
                continue;
            }
            
            universalDeviceData.FirstReadingDtm = device.SensorData.Min(f => f.DateTime);
            universalDeviceData.LastReadingDtm = device.SensorData.Max(f => f.DateTime);

            //Calculations
            universalDeviceData.TemperatureCount =
                device.SensorData.Count(f => f.SensorType == ModelConstants.FOO2_TEMPERATURE);
            
            universalDeviceData.AverageTemperature =
                device.SensorData.Where(f => f.SensorType == ModelConstants.FOO2_TEMPERATURE).Average(g => g.Value);
            
            universalDeviceData.HumidityCount =
                device.SensorData.Count(f => f.SensorType == ModelConstants.FOO2_HUMIDITY);
            
            universalDeviceData.AverageHumidity =
                device.SensorData.Where(f => f.SensorType == ModelConstants.FOO2_HUMIDITY).Average(g => g.Value);

            universalDeviceDataList.Add(universalDeviceData);
        }

        return universalDeviceDataList;
    }
}