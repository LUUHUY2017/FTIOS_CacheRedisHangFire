namespace AMMS.Hanet.Data;

public class AccessToken
{
    public string access_token { get; set; }
    public string refresh_token { get; set; }
    public string email { get; set; }
    public string userID { get; set; }
    public long expire { get; set; }
    public DateTime TimeExpires
    {
        get
        {
            return new DateTime(expire);
        }
    }
    public string token_type { get; set; }
}
public class HanetParam
{
    public static string Host { get; set; } = "https://oauth.hanet.com/";
    public static AccessToken Token { get; set; } = new AccessToken()
    {
        refresh_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjQxMjYyMTM0MDE3Mjc3NDY2NDEiLCJlbWFpbCI6Im5hbW5kQGFjcy52biIsImNsaWVudF9pZCI6ImUwZjM0NWRhNWYxODdkMjZiMDE4ZTFkMzYwM2FkOGE4IiwidHlwZSI6InJlZnJlc2hfdG9rZW4iLCJpYXQiOjE3MzAxODYxOTAsImV4cCI6MTc2MTcyMjE5MH0.jhX-qtVsjM0wmPShdGbUC5oOB9_d-tzN1To6ef4SvGs",
        access_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjQxMjYyMTM0MDE3Mjc3NDY2NDEiLCJlbWFpbCI6Im5hbW5kQGFjcy52biIsImNsaWVudF9pZCI6ImUwZjM0NWRhNWYxODdkMjZiMDE4ZTFkMzYwM2FkOGE4IiwidHlwZSI6ImF1dGhvcml6YXRpb25fY29kZSIsImlhdCI6MTczMDE4NjE5MCwiZXhwIjoxNzYxNzIyMTkwfQ.Bky9h5CEkfySYcHW6faKPenLkw6XA5XCBznhhDnMRQ8",
        expire = 1761709129,
        token_type = "bearer"
    };

}