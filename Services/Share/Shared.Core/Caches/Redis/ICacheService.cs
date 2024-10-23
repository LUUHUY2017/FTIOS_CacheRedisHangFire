namespace Shared.Core.Caches.Redis;

public interface ICacheService
{
    /// <summary>
    /// Get Data using keyName
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keyName"></param>
    /// <returns></returns>
    Task<T> GetData<T>(string keyName);

    /// <summary>
    /// Set Data with Value and Expiration Time of Key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keyName"></param>
    /// <param name="value"></param>
    /// <param name="expirationTime"></param>
    /// <returns></returns>
    Task<T> SetData<T>(string keyName, T value, DateTimeOffset expirationTime);

    /// <summary>
    /// Remove Data
    /// </summary>
    /// <param name="keyName"></param>
    /// <returns></returns>
    Task<bool> RemoveData(string keyName);

    Task<bool> Removes(string pattern);

    Task<List<T>> Gets<T>(string keyName);

     

}
