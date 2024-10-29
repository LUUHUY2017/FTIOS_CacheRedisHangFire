using Newtonsoft.Json;
using RestSharp;
using Shared.Core.Identity.User;

namespace AMMS.WebApp.Share.Services;
public class AMMS_Client_Call_API
{
    public static string? Host = "";
    public static string? AccessToken = "";

    public static UserAccountGeneralResponse GetPageApiByUser(string host, string accessToken, string userId)
    {
        var accInfo = new UserAccountGeneralResponse();
        try
        {
            var options = new RestClientOptions($"{host}") { MaxTimeout = -1, };
            var client = new RestClient(options);
            var request = new RestRequest($"/api/v1/User/GetPagesById?id={userId}", Method.Get);
            request.AddHeader("Authorization", "Bearer " + accessToken);
            RestResponse response = client.Execute(request);
            var jsonString = response.Content;
            if (response.IsSuccessful && !string.IsNullOrWhiteSpace(jsonString))
            {
                var items = JsonConvert.DeserializeObject<UserAccountInfoModel>(jsonString);
                if (items != null && items.Succeeded && items.Data != null)
                    accInfo = items.Data;
            }
        }
        catch (Exception e) { }
        return accInfo;
    }

}
