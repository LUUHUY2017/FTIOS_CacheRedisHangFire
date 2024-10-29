namespace AMMS.Hanet.Applications.AppConfigs.V1.Models;

public class AppConfigResponse
{
    public string? Id { get; set; }
    public string? ClientScret { get; set; }
    public string? ClientId { get; set; }
    public string? UserId { get; set; }
    public string? Email { get; set; }
    public long? Expire { get; set; } = 0;
    public string? TokenType { get; set; }
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public string? Description { get; set; }
    public string? GrantType { get; set; }
}
