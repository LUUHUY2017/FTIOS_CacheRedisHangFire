using AMMS.VIETTEL.SMAS.Cores.Entities.A0;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;

namespace AMMS.VIETTEL.SMAS.Applications.Services.AppConfigs.V1.Models;

public class AppeConfigResponse
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
    public DateTime? TimeAsync { get; set; }

    public string? OrganizationId { get; set; }
    public string? OrganizationName { get; set; }   
    public string? Note { get; set; }

    public string? Key { get; set; }
    public string? KeyIV { get; set; }
    public string? SecretKey { get; set; }
    public AppeConfigResponse()
    {
        
    }
    public AppeConfigResponse(AttendanceConfig cf, Organization o)
    {
        Id = cf.Id;
        EndpointIdentity = cf.EndpointIdentity;
        AccountName = cf.AccountName;
        Password = cf.Password;
        GrantType = cf.GrantType;
        Scope = cf.Scope;
        ClientId = cf.ClientId;
        ClientSecret = cf.ClientSecret;
        EndpointGateway = cf.EndpointGateway;
        TimeAsync = cf.TimeAsync;
        OrganizationId = cf.OrganizationId;
        OrganizationName = o?.OrganizationName;
        Key = cf.Key;
        KeyIV = cf.KeyIV;
        SecretKey = cf.SecretKey;
    }
}
