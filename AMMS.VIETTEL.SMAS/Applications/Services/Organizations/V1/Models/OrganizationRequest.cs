﻿namespace AMMS.VIETTEL.SMAS.Applications.Services.Organizations.V1.Models;

public class OrganizationRequest
{
    public string? Id { get; set; } = "";
    public bool? Actived { get; set; }
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