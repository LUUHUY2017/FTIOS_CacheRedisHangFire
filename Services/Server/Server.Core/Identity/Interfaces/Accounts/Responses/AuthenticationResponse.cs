namespace Server.Core.Identity.Interfaces.Accounts.Responses;

public class AuthenticationResponse
{
    public string Id { get; set; } = "";
    public string UserName { get; set; } = "";
    public string FirtName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public IList<string> Roles { get; set; } = new List<string>();
    public IList<string> Scopes { get; set; } = new List<string>();
    public bool IsVerified { get; set; } = false;
    public string AccessToken { get; set; } = "";
    public string TokenType { get; set; } = "";
    public string RefreshToken { get; set; } = "";
    public DateTime ExpiresTime { get; set; }

    public long ExpiresIn { get; set; }
    public string Scope { get; set; } = "";
}
