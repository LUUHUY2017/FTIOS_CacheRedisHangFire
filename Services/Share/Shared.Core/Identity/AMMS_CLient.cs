using Newtonsoft.Json;
using Shared.Core.Commons;

namespace Shared.Core.Identity;

public class IndentityClientParam
{
    public string host { get; set; }
    public string client_id { get; set; }
    public string client_secret { get; set; }
    public string grant_type { get; set; }

    public string username { get; set; }
    public string password { get; set; }

}

public class AMMS_Client
{
    public string Host { get; private set; }
    private IndentityClientParam _clientParam { get; set; }
    private AccessToken _accessToken { get; set; }

    public void SetParam(IndentityClientParam clientParam)
    {
        _clientParam = clientParam;
        Host = _clientParam.host;
    }
    public async Task<Result<AccessToken>> GetAccessToken()
    {
        if (_accessToken == null || _accessToken.expires_time <= DateTime.UtcNow)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"{_clientParam.host}/connect/token");
            var collection = new List<KeyValuePair<string, string>>();
            collection.Add(new("client_id", _clientParam.client_id));
            collection.Add(new("client_secret", _clientParam.client_secret));
            collection.Add(new("grant_type", _clientParam.grant_type));
            if (_clientParam.grant_type == "password")
            {
                collection.Add(new("username", _clientParam.username));
                collection.Add(new("password", _clientParam.password));
            }
            var content = new FormUrlEncodedContent(collection);
            request.Content = content;
            var response = await client.SendAsync(request);

            //var client = new HttpClient();
            //var request = new HttpRequestMessage(HttpMethod.Post, "https://amms.acs.vn:8000/connect/token");
            //var collection = new List<KeyValuePair<string, string>>();
            //collection.Add(new("client_id", "amms.delivery.api"));
            //collection.Add(new("client_secret", "secret"));
            //collection.Add(new("grant_type", "client_credentials"));
            //var content = new FormUrlEncodedContent(collection);
            //request.Content = content;
            //var response = await client.SendAsync(request);


            var retContent = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                _accessToken = JsonConvert.DeserializeObject<AccessToken>(retContent);
            }
        }
        if (_accessToken == null)
            return new Result<AccessToken>(_accessToken, "Không lấy được token", false);
        return new Result<AccessToken>(_accessToken, "Thành công", true);
    }
    public async Task<Result<AccessToken>> RefreshToken()
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_clientParam.host}/connect/token");
        var collection = new List<KeyValuePair<string, string>>();
        collection.Add(new("client_id", _clientParam.client_id));
        collection.Add(new("client_secret", _clientParam.client_secret));
        collection.Add(new("grant_type", "refresh_token"));
        collection.Add(new("refresh_token", _accessToken.refresh_token));
        var content = new FormUrlEncodedContent(collection);
        request.Content = content;
        var response = await client.SendAsync(request);
        var retContent = await response.Content.ReadAsStringAsync();
        if (response.StatusCode == System.Net.HttpStatusCode.OK)
        {
            RefreshToken accessToken = JsonConvert.DeserializeObject<RefreshToken>(retContent);
            return new Result<AccessToken>(accessToken, "Thành công", true);
        }
        else
        {
            return new Result<AccessToken>(null, "Có lỗi: " + retContent, false);
        }

    }

}
