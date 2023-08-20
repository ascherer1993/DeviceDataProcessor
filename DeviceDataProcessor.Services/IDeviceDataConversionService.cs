using DeviceDataProcessor.Models;

namespace DeviceDataProcessor.Services;

public interface IDeviceDataConversionService
{
    public List<UniversalDeviceData> ConvertJsonToUnifiedDeviceData(params string[] jsonFiles);

    public List<UniversalDeviceData> ConvertJsonToUnifiedDeviceData(List<string> jsonFiles);
}