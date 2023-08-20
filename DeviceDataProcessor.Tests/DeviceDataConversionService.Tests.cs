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
            deviceDataList.Count == 4
            && deviceDataList.Count(universalDeviceData => universalDeviceData.CompanyName == "Foo1") == 0
            && deviceDataList.Count(universalDeviceData => universalDeviceData.CompanyName == "Foo2") == 4
        )), Times.Once);
    }
    
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
    }
    
    [Fact]
    public void InvalidSchema()
    {
        _sut.ConvertJsonToUnifiedDeviceData(_validJsonFoo1, _invalidJsonFoo3);
    }
    
    [Fact]
    public void BadJson()
    {
        _sut.ConvertJsonToUnifiedDeviceData(_validJsonFoo1, _badJsonFoo4);
    }
}