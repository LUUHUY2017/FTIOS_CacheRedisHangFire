using Server.Core.MongoDB.Entities;

namespace Server.Core.MongoDB.Repositories;

public interface IPOC_DataInOutEventRepository
{
    //Task<Pagination<Product>> Gets(CatalogSpecParams catalogSpecParams);

    Task<List<POC_DataInOutEvent>> GetAlls(string serialNumber);
    Task<POC_DataInOutEvent> Get(string serialNumber, long unixStartTime);

    /// <summary>
    /// Lấy dữ liệu chưa được xử lý
    /// </summary>
    /// <param name="serialNumber"></param>
    /// <param name="unixStartTime"></param>
    /// <returns></returns>
    Task<List<POC_DataInOutEvent>> Get_Unprocess(string serialNumber, long unixStartTime);
    Task<POC_DataInOutEvent> CreateAsync(POC_DataInOutEvent data);
    Task<List<POC_DataInOutEvent>> CreateAsync(List<POC_DataInOutEvent> datas);
    Task<bool> UpdateAsync(POC_DataInOutEvent product);
    Task<bool> UpdateAsync(List<POC_DataInOutEvent> datas);
    Task<bool> DeleteAsync(string id);
}
