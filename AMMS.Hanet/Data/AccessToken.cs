namespace AMMS.Hanet.Data;

public class AccessToken
{
    public string access_token { get; set; }
    public string refresh_token { get; set; }
    public string email { get; set; }
    public string userID { get; set; }
    public long expire { get; set; }
    public string token_type { get; set; }
}
public class HanetParam
{
    public static string Host { get; set; }
    public static AccessToken Token { get; set; }

}