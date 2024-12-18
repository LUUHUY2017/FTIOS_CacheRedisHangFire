﻿namespace Server.Application.MasterDatas.A0.AttendanceTimeConfigs.V1.Models;

public class AttendanceTimeConfigFilter
{
    public string? OrganizationId { get; set; } = string.Empty;
    public string? Key { get; set; } = string.Empty;
    public string? Type { get; set; } = string.Empty;
    public bool? Actived { get; set; } = true;
    public string? ColumnTable { get; set; } = string.Empty;
}
