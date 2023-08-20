using DeviceDataProcessor.Models;

namespace DeviceDataProcessor.Tests.TestData;

public class UniversalDeviceDataTests
{
    private const int COMPANY_ID = 1;
    private const string COMPANY_NAME = "TestCompany";
    
    private const int DEVICE_ID = 10;
    private const int DEVICE_ID_2 = 11;
    private const string DEVICE_NAME = "TestDevice";
    private const string DEVICE_NAME_2 = "TestDevice2";

    private static DateTime _shipmentDate = new DateTime(2019, 1, 1, 10, 1, 1);
    private static DateTime _firstReadingDate = new DateTime(2020, 1, 1, 10, 1, 1);
    private static DateTime _lastReadingDate = new DateTime(2021, 1, 1, 10, 1, 1);
    private static DateTime _notFirstOrLastReadingDate = new DateTime(2020, 6, 15, 10, 1, 1);
    
    
    [Fact]
    public void ConvertToUniversalFromFoo1_CorrectData()
    {
        //Arrange
        const double temp1Value = 30;
        const double temp2Value = 40;
        const double averageTemp = 35;

        const double humidity1Value = 90;
        const double humidity2Value = 100;
        const double averageHumidity = 95;
        
        Foo1Dto foo1Dto = new Foo1Dto()
        {
            PartnerId = COMPANY_ID,
            PartnerName = COMPANY_NAME,
            Trackers = new List<Foo1Tracker>()
            {
                new()
                {
                    Id = DEVICE_ID,
                    Model = DEVICE_NAME,
                    ShipmentStartDtm = _shipmentDate,
                    Sensors = new List<Foo1Sensor>()
                    {
                        new()
                        {
                            Id = 1,
                            Name = ModelConstants.FOO1_TEMPERATURE,
                            Crumbs = new List<Foo1Crumb>()
                            {
                                new()
                                {
                                    CreatedDtm = _notFirstOrLastReadingDate,
                                    Value = temp1Value
                                },
                                new()
                                {
                                    CreatedDtm = _lastReadingDate,
                                    Value = temp2Value
                                }
                            }
                        },
                        new()
                        {
                            Id = 2,
                            Name = ModelConstants.FOO1_HUMIDTY,
                            Crumbs = new List<Foo1Crumb>()
                            {
                                new()
                                {
                                    CreatedDtm = _firstReadingDate,
                                    Value = humidity1Value
                                },
                                new()
                                {
                                    CreatedDtm = _notFirstOrLastReadingDate,
                                    Value = humidity2Value
                                }
                            }
                        }
                    }
                }
            }
        };
        
        //Act
        List<UniversalDeviceData> response = UniversalDeviceData.GetUniversalDeviceDataFromDto(foo1Dto);
        
        //Assert
        Assert.Single(response);
        
        UniversalDeviceData data = response.First();
        Assert.Equal(COMPANY_ID, data.CompanyId);
        Assert.Equal(COMPANY_NAME, data.CompanyName);
        Assert.Equal(DEVICE_ID, data.DeviceId);
        Assert.Equal(DEVICE_NAME, data.DeviceName);
        Assert.Equal(_firstReadingDate, data.FirstReadingDtm);
        Assert.Equal(_lastReadingDate, data.LastReadingDtm);
        Assert.Equal(2, data.TemperatureCount);
        Assert.Equal(2, data.HumidityCount);
        Assert.Equal(averageTemp, data.AverageTemperature);
        Assert.Equal(averageHumidity, data.AverageHumidity);
    }
    
    
    [Fact]
    public void ConvertToUniversalFromFoo2_CorrectData()
    {
        //Arrange
        const double temp1Value = 30;
        const double temp2Value = 40;
        const double averageTemp = 35;

        const double humidity1Value = 90;
        const double humidity2Value = 100;
        const double averageHumidity = 95;
        
        Foo2Dto foo2Dto = new Foo2Dto()
        {
            CompanyId = COMPANY_ID,
            Company = COMPANY_NAME,
            Devices = new List<Foo2Device>()
            {
                new Foo2Device()
                {
                    DeviceId = DEVICE_ID,
                    Name = DEVICE_NAME,
                    StartDateTime = _firstReadingDate,
                    SensorData = new List<Foo2SensorData>()
                    {
                        new()
                        {
                            DateTime = _firstReadingDate,
                            SensorType = ModelConstants.FOO2_TEMPERATURE,
                            Value = temp1Value
                        },
                        new()
                        {
                            DateTime = _notFirstOrLastReadingDate,
                            SensorType = ModelConstants.FOO2_TEMPERATURE,
                            Value = temp2Value
                        },
                        new()
                        {
                            DateTime = _notFirstOrLastReadingDate,
                            SensorType = ModelConstants.FOO2_HUMIDITY,
                            Value = humidity1Value
                        },
                        new()
                        {
                            DateTime = _lastReadingDate,
                            SensorType = ModelConstants.FOO2_HUMIDITY,
                            Value = humidity2Value
                        }
                    }
                }
            }
        };
        
        //Act
        List<UniversalDeviceData> response = UniversalDeviceData.GetUniversalDeviceDataFromDto(foo2Dto);
        
        //Assert
        Assert.Single(response);
        
        UniversalDeviceData data = response.First();
        Assert.Equal(COMPANY_ID, data.CompanyId);
        Assert.Equal(COMPANY_NAME, data.CompanyName);
        Assert.Equal(DEVICE_ID, data.DeviceId);
        Assert.Equal(DEVICE_NAME, data.DeviceName);
        Assert.Equal(_firstReadingDate, data.FirstReadingDtm);
        Assert.Equal(_lastReadingDate, data.LastReadingDtm);
        Assert.Equal(2, data.TemperatureCount);
        Assert.Equal(2, data.HumidityCount);
        Assert.Equal(averageTemp, data.AverageTemperature);
        Assert.Equal(averageHumidity, data.AverageHumidity);
    }
}