﻿using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Core.Entities.A0;

[Table("PersonSynToDevice")]
public class A2_PersonSynToDevice : EntityBase
{
    [MaxLength(50)]
    public string? PersonId { get; set; }
    [MaxLength(50)]
    public string? DeviceId { get; set; }
    [MaxLength(50)]
    public string? SynAction { get; set; }
    public bool? SynStatus { get; set; }
    public string? SynMessage { get; set; }

    public bool? SynFaceStatus { get; set; }
    public string? SynFaceMessage { get; set; }

    public bool? SynFingerStatus { get; set; }
    public string? SynFingerMessage { get; set; }

    public bool? SynCardStatus { get; set; }
    public string? SynCardMessage { get; set; }
}