using DeviceDataProcessor.DataAccess;
using DeviceDataProcessor.Models;
using DeviceDataProcessor.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace DeviceDataProcessor.Tests;

public class DeviceDataConversationServiceTests
{
    private DeviceDataConversionService _sut;

    private readonly Mock<IDeviceDataRepository> _dataRepository;
    private readonly Mock<ILogger<DeviceDataConversionService>> _logger;

    private string _validJsonFoo1;
    private string _validJsonFoo2;
    private string _invalidJsonFoo3;
    private string _badJsonFoo4;
    private string _validJsonFoo5;
    private string _validJsonFoo6_NoSensorData;
    private string _validJsonFoo7_NoTrackers;
    
    public DeviceDataConversationServiceTests()
    {
        LoadFiles();

        //Mock dependencies
        _dataRepository = new Mock<IDeviceDataRepository>();
        _logger = new Mock<ILogger<DeviceDataConversionService>>();
        
        _dataRepository.Setup(f => f.AddDeviceDataRangeAndSave(It.IsAny<List<UniversalDeviceData>>()))
            .Returns(new List<UniversalDeviceData>());

        _sut = new DeviceDataConversionService(_dataRepository.Object, _logger.Object);
    }

    private void LoadFiles()
    {
        string currentPath = Directory.GetCurrentDirectory();

        _validJsonFoo1 = GetJsonFromFileIfExists(currentPath + "/TestData/DeviceDataFoo1.json");
        _validJsonFoo2 = GetJsonFromFileIfExists(currentPath + "/TestData/DeviceDataFoo2.json");
        _invalidJsonFoo3 = GetJsonFromFileIfExists(currentPath + "/TestData/DeviceDataFoo3-InvalidSchema.json");
        _badJsonFoo4 = GetJsonFromFileIfExists(currentPath + "/TestData/DeviceDataFoo4-BadJson.json");
        _validJsonFoo5 = GetJsonFromFileIfExists(currentPath + "/TestData/DeviceDataFoo5.json");
        _validJsonFoo6_NoSensorData = GetJsonFromFileIfExists(currentPath + "/TestData/DeviceDataFoo6-NoSensorData.json");
        _validJsonFoo7_NoTrackers = GetJsonFromFileIfExists(currentPath + "/TestData/DeviceDataFoo7-NoTrackers.json");
    }

    private string GetJsonFromFileIfExists(string path)
    {
        if (File.Exists(path))
        {
            using StreamReader streamReader = new(path);
            return streamReader.ReadToEnd();
        }
        Assert.True(false, "Valid path for file was not provided");
        return "";
    }
    
    /// <summary>
    /// Sends two json files. One of schema Foo1 and another of schema Foo2.
    /// Checks what repo is called with since method returns the response from repo
    /// </summary>
    [Fact]
    public void ValidJson_TwoFilesWithDifferentSchemas_ShouldReturnCombinedList()
    {
        //Act
        List<UniversalDeviceData> response = _sut.ConvertJsonToUnifiedDeviceData(_validJsonFoo1, _validJsonFoo2);
        
        //Assert
        _dataRepository.Verify(f => f.AddDeviceDataRangeAndSave(It.Is<List<UniversalDeviceData>>(deviceDataList => 
            deviceDataList.Count == 4
            && deviceDataList.Count(universalDeviceData => universalDeviceData.CompanyName == "Foo1") == 2
            && deviceDataList.Count(universalDeviceData => universalDeviceData.CompanyName == "Foo2") == 2
            )), Times.Once);
        
        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel =>
                    logLevel == LogLevel.Warning || logLevel == LogLevel.Error || logLevel == LogLevel.Critical),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);
    }
    
    /// <summary>
    /// Sends two json files. Both use schema Foo2.
    /// Checks what repo is called with since method returns the response from repo
    /// </summary>
    [Fact]
    public void ValidJson_TwoFilesWithSameSchema_ShouldReturnCombinedList()
    {
        //Act
        List<UniversalDeviceData> response = _sut.ConvertJsonToUnifiedDeviceData(_validJsonFoo2, _validJsonFoo5);
        
        //Assert
        _dataRepository.Verify(f => f.AddDeviceDataRangeAndSave(It.Is<List<UniversalDeviceData>>(deviceDataList => 
            deviceDataList.Count == 3
            && deviceDataList.Count(universalDeviceData => universalDeviceData.CompanyName == "Foo1") == 0
            && deviceDataList.Count(universalDeviceData => universalDeviceData.CompanyName == "Foo2") == 3
        )), Times.Once);
        
        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel =>
                    logLevel == LogLevel.Warning || logLevel == LogLevel.Error || logLevel == LogLevel.Critical),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);
    }
    
    /// <summary>
    /// Sends three json files. One of schema Foo1 and two are of schema Foo2.
    /// Checks what repo is called with since method returns the response from repo
    /// </summary>
    [Fact]
    public void ValidJson_ThreeFiles_ShouldReturnCombinedList()
    {
        //Act
        List<UniversalDeviceData> response = _sut.ConvertJsonToUnifiedDeviceData(_validJsonFoo1, _validJsonFoo2, _validJsonFoo5);
        
        //Assert
        _dataRepository.Verify(f => f.AddDeviceDataRangeAndSave(It.Is<List<UniversalDeviceData>>(deviceDataList => 
            deviceDataList.Count == 5
            && deviceDataList.Count(universalDeviceData => universalDeviceData.CompanyName == "Foo1") == 2
            && deviceDataList.Count(universalDeviceData => universalDeviceData.CompanyName == "Foo2") == 3
        )), Times.Once);

        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel =>
                    logLevel == LogLevel.Warning || logLevel == LogLevel.Error || logLevel == LogLevel.Critical),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);
    }

    /// <summary>
    /// Sends two json files. One of the files does not match a schema and is skipped after the exception is caught
    /// Checks what repo is called with since method returns the response from repo
    /// </summary>
    [Fact]
    public void InvalidSchema_FileIsSkipped()
    {
        //Act
        _sut.ConvertJsonToUnifiedDeviceData(_validJsonFoo1, _invalidJsonFoo3);

        //Assert
        _dataRepository.Verify(f => f.AddDeviceDataRangeAndSave(It.Is<List<UniversalDeviceData>>(deviceDataList =>
            deviceDataList.Count == 2
            && deviceDataList.Count(universalDeviceData => universalDeviceData.CompanyName == "Foo1") == 2
        )), Times.Once);

        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }

    /// <summary>
    /// Sends two json files. One of the files contains invalid json and is skipped after the exception is caught
    /// Checks what repo is called with since method returns the response from repo
    /// </summary>
    [Fact]
    public void BadJson_FileIsSkipped()
    {
        //Act
        _sut.ConvertJsonToUnifiedDeviceData(_validJsonFoo1, _badJsonFoo4);
        
        //Assert
        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel => logLevel == LogLevel.Warning),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Once);
    }
    
    /// <summary>
    /// Sends two json files. One of the files contains no sensor data and is skipped
    /// Checks what repo is called with since method returns the response from repo
    /// </summary>
    [Fact]
    public void NoSensorData_FileIsIgnored()
    {
        //Act
        _sut.ConvertJsonToUnifiedDeviceData(_validJsonFoo1, _validJsonFoo6_NoSensorData);
        
        //Assert
        _dataRepository.Verify(f => f.AddDeviceDataRangeAndSave(It.Is<List<UniversalDeviceData>>(deviceDataList =>
            deviceDataList.Count == 2
            && deviceDataList.Count(universalDeviceData => universalDeviceData.CompanyName == "Foo1") == 2
        )), Times.Once);

        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel =>
                    logLevel == LogLevel.Warning || logLevel == LogLevel.Error || logLevel == LogLevel.Critical),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);
    }
    
    /// <summary>
    /// Sends two json files. One of the files contains no trackers and is skipped
    /// Checks what repo is called with since method returns the response from repo
    /// </summary>
    [Fact]
    public void NoTrackers_FileIsIgnored()
    {
        //Act
        _sut.ConvertJsonToUnifiedDeviceData(_validJsonFoo1, _validJsonFoo7_NoTrackers);
        
        //Assert
        _dataRepository.Verify(f => f.AddDeviceDataRangeAndSave(It.Is<List<UniversalDeviceData>>(deviceDataList =>
            deviceDataList.Count == 2
            && deviceDataList.Count(universalDeviceData => universalDeviceData.CompanyName == "Foo1") == 2
        )), Times.Once);

        _logger.Verify(logger => logger.Log(
                It.Is<LogLevel>(logLevel =>
                    logLevel == LogLevel.Warning || logLevel == LogLevel.Error || logLevel == LogLevel.Critical),
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.Never);
    }
}