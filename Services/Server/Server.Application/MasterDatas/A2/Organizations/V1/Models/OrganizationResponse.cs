using System.ComponentModel.DataAnnotations;

namespace Server.Application.MasterDatas.A2.Organizations.V1.Models;

public class OrganizationResponse
{
    public string? Id { get; set; } = "";
    public string? OrganizationCode { get; set; } = "";
    public string? OrganizationShortName { get; set; } = "";
    public string? OrganizationName { get; set; } = "";
    public string? OrganizationAddress { get; set; } = "";
    public string? OrganizationPhone { get; set; } = "";
    public string? OrganizationFax { get; set; } = "";

    public string? OrganizationEmail { get; set; } = "";
    public string? OrganizationDescription { get; set; } = "";
    public string? OrganizationNote { get; set; } = "";
}
