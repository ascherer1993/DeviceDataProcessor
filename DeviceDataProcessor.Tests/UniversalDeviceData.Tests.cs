using DeviceDataProcessor.Models;

namespace DeviceDataProcessor.Tests;

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
    
    /// <summary>
    /// Tests that averages and counts are calculated correctly for foo1
    /// </summary>
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

    /// <summary>
    /// Tests that averages and counts are calculated correctly for foo1
    /// </summary>
    [Fact]
    public void ConvertToUniversalFromFoo1_MultipleTrackers_CorrectData()
    {
        //Arrange
        const double temp1Value = 30;
        const double temp2Value = 40;
        const double temp3Value = 20;
        const double averageTemp = 35;
        const double averageTemp2 = 30;

        const double humidity1Value = 90;
        const double humidity2Value = 100;
        const double humidity3Value = 80;
        const double averageHumidity = 95;
        const double averageHumidity2 = 90;
        
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
                },
                new()
                {
                    Id = DEVICE_ID_2,
                    Model = DEVICE_NAME_2,
                    ShipmentStartDtm = _shipmentDate,
                    Sensors = new List<Foo1Sensor>()
                    {
                        new()
                        {
                            Id = 10,
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
                                    CreatedDtm = _notFirstOrLastReadingDate,
                                    Value = temp2Value
                                },
                                new()
                                {
                                    CreatedDtm = _lastReadingDate,
                                    Value = temp3Value
                                }
                            }
                        },
                        new()
                        {
                            Id = 11,
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
                                },
                                new()
                                {
                                    CreatedDtm = _notFirstOrLastReadingDate,
                                    Value = humidity3Value
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
        Assert.Equal(2, response.Count);
        
        UniversalDeviceData data = response.First(f => f.DeviceName == DEVICE_NAME);
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
        
        UniversalDeviceData data2 = response.First(f => f.DeviceName == DEVICE_NAME_2);
        Assert.Equal(COMPANY_ID, data2.CompanyId);
        Assert.Equal(COMPANY_NAME, data2.CompanyName);
        Assert.Equal(DEVICE_ID_2, data2.DeviceId);
        Assert.Equal(DEVICE_NAME_2, data2.DeviceName);
        Assert.Equal(_firstReadingDate, data2.FirstReadingDtm);
        Assert.Equal(_lastReadingDate, data2.LastReadingDtm);
        Assert.Equal(3, data2.TemperatureCount);
        Assert.Equal(3, data2.HumidityCount);
        Assert.Equal(averageTemp2, data2.AverageTemperature);
        Assert.Equal(averageHumidity2, data2.AverageHumidity);
    }
    
    /// <summary>
    /// Tests that averages and counts are calculated correctly for foo2
    /// </summary>
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
    
    /// <summary>
    /// Tests that averages and counts are calculated correctly for foo2
    /// </summary>
    [Fact]
    public void ConvertToUniversalFromFoo2_MultipleDevices_CorrectData()
    {
        //Arrange
        const double temp1Value = 30;
        const double temp2Value = 40;
        const double temp3Value = 20;
        const double averageTemp = 35;
        const double averageTemp2 = 30;

        const double humidity1Value = 90;
        const double humidity2Value = 100;
        const double humidity3Value = 80;
        const double averageHumidity = 95;
        const double averageHumidity2 = 90;
        
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
                },
                new Foo2Device()
                {
                    DeviceId = DEVICE_ID_2,
                    Name = DEVICE_NAME_2,
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
                            SensorType = ModelConstants.FOO2_TEMPERATURE,
                            Value = temp3Value
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
                        },
                        new()
                        {
                            DateTime = _lastReadingDate,
                            SensorType = ModelConstants.FOO2_HUMIDITY,
                            Value = humidity3Value
                        }
                    }
                }
            }
        };
        
        //Act
        List<UniversalDeviceData> response = UniversalDeviceData.GetUniversalDeviceDataFromDto(foo2Dto);
        
        //Assert
        Assert.Equal(2, response.Count);
        
        UniversalDeviceData data = response.First(f => f.DeviceName == DEVICE_NAME);
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
        
        UniversalDeviceData data2 = response.First(f => f.DeviceName == DEVICE_NAME_2);
        Assert.Equal(COMPANY_ID, data2.CompanyId);
        Assert.Equal(COMPANY_NAME, data2.CompanyName);
        Assert.Equal(DEVICE_ID_2, data2.DeviceId);
        Assert.Equal(DEVICE_NAME_2, data2.DeviceName);
        Assert.Equal(_firstReadingDate, data2.FirstReadingDtm);
        Assert.Equal(_lastReadingDate, data2.LastReadingDtm);
        Assert.Equal(3, data2.TemperatureCount);
        Assert.Equal(3, data2.HumidityCount);
        Assert.Equal(averageTemp2, data2.AverageTemperature);
        Assert.Equal(averageHumidity2, data2.AverageHumidity);
    }
}