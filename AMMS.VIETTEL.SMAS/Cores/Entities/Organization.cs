﻿using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.VIETTEL.SMAS.Cores.Entities;


[Table("A2_Organization")]
public class Organization : EntityBase
{
    [MaxLength(250)]
    public string? OrganizationCode { get; set; }
    [MaxLength(500)]
    public string? OrganizationShortName { get; set; }
    [MaxLength(500)]
    public string? OrganizationName { get; set; }
    [MaxLength(500)]
    public string? OrganizationAddress { get; set; }
    [MaxLength(500)]
    public string? OrganizationTax { get; set; }
    [MaxLength(250)]
    public string? OrganizationPhone { get; set; }
    [MaxLength(250)]
    public string? OrganizationFax { get; set; }

    [MaxLength(250)]
    public string? OrganizationEmail { get; set; }
    [MaxLength(500)]
    public string? OrganizationDescription { get; set; }
    [MaxLength(500)]
    public string? OrganizationNote { get; set; }


    public string? OrganizationTypeId { get; set; }

    public string? OrganizationLogo { get; set; }
    public string? OrganizationFavicon { get; set; }

    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}