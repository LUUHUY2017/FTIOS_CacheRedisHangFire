﻿using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.VIETTEL.SMAS.Cores.Entities.A0;

[Table("Attendanceconfig")]
public class AttendanceConfig : EntityBase
{
    public string? EndpointIdentity { get; set; }
    public string? AccountName { get; set; }
    public string? Password { get; set; }
    public string? GrantType { get; set; }
    public string? Scope { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }

    public string? EndpointGateway { get; set; }
    public DateTime? TimeAsync { get; set; }
    public string? Note { get; set; }

    public string? Key { get; set; }
    public string? KeyIV { get; set; }
    public string? SecretKey { get; set; }

    public string? access_token { get; set; }
    public string? refresh_token { get; set; }
    public int? expires_in { get; set; }
    public string? token_type { get; set; }
    public DateTime? time_expires_in { get; set; }

}