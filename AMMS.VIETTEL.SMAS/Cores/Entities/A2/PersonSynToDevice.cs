using Shared.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace AMMS.VIETTEL.SMAS.Cores.Entities.A2;

public class PersonSynToDevice : EntityBase
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