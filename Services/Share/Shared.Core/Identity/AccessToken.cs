using Newtonsoft.Json;
using Shared.Core.Commons;

namespace Shared.Core.Identity;

public class AccessToken
{
    public string access_token { get; set; }
    public string refresh_token { get; set; }

    int _expires_in { get; set; }
    public int expires_in { get { return _expires_in; }  set { _expires_in = value; expires_time = DateTime.UtcNow.AddSeconds(value); } }
    public string token_type { get; set; }
    public string scope { get; set; }

    /// <summary>
    /// UTC 0
    /// </summary>

    public DateTime expires_time { get; private set; }

}
public class RefreshToken : AccessToken
{
    public string id_token { get; set; }
}

public class AccountService
{
    public static async Task<Result<AccessToken>> Login_Password(string host, string client_id, string client_secret, string username, string password)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, $"{host}/connect/token");
        var collection = new List<KeyValuePair<string, string>>();
        collection.Add(new("client_id", client_id));
        collection.Add(new("client_secret", client_secret));
        collection.Add(new("grant_type", "password"));
        collection.Add(new("username", username));
        collection.Add(new("password", password));
        var content = new FormUrlEncodedContent(collection);
        request.Content = content;

        var response = await client.SendAsync(request);

        string json = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
            return new Result<AccessToken>(JsonConvert.DeserializeObject<AccessToken>(json), "Success!", true);
        return new Result<AccessToken>(null, json, false);
    }
}