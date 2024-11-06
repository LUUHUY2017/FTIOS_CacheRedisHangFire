namespace Server.Application.MasterDatas.A0.AttendanceConfigs.V1.Models;

public class AttendanceConfigRequest
{
    public string? Id { get; set; }
    public string? EndpointIdentity { get; set; }
    public string? AccountName { get; set; }
    public string? Password { get; set; }
    public string? GrantType { get; set; }
    public string? Scope { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? EndpointGateway { get; set; }

    public string? OrganizationId { get; set; }

    public string? Note { get; set; }
}
