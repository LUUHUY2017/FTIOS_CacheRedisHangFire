namespace Server.Core.Caches.Redis;

//public interface IDeviceCacheService
//{
//    Task<List<Terminal>> Get_Terminal_BrickStream();
//    Task<bool> Set_Terminal_BrickStream(List<Terminal> datas, DateTime? expirationTime = null);
//    Task<bool> Remove_Terminal_BrickStream();
//}
//public interface ISiteCacheService
//{
//   Task<List<Site>> Get_Sites();
//    Task<bool> Set_Sites(List<Site> datas, DateTime? expirationTime = null);
//    Task<bool> Remove_Sites();
//}
//public interface  IPocDataInOutLogCacheService
//{ 
//    #region DataInOutLogs - Lịch sử dữ liệu vào ra theo từng thiết bị
//    IEnumerable<PocDataInOutLog> Get_DataInOutLogs(string sn);
//    bool Set_DataInOutLogs(IEnumerable<PocDataInOutLog> datas, DateTime? expirationTime = null, string sn = null);
//    bool Remove_DataInOutLogs(string sn);
//    #endregion
//}