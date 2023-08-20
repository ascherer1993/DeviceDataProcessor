using DeviceDataProcessor.Models;

namespace DeviceDataProcessor.DataAccess;

public interface IDeviceDataRepository
{
    public List<UniversalDeviceData> AddDeviceDataRangeAndSave(List<UniversalDeviceData> universalDeviceDataList);
}