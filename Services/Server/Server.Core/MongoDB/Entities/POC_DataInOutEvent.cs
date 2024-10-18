using Server.Core.MongoDB.Entities.Base;

namespace Server.Core.MongoDB.Entities;

public partial class POC_DataInOutEvent : BaseEntity
{
    //public int OrganizationId { get; set; }

    //public int SiteId { get; set; }

    //public int LocationId { get; set; }

    public string SerialNumber { get; set; } = null!;

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public int Interval { get; set; }

    public int TimeId { get; set; }

    public int? NumToEnter { get; set; }

    public int? NumToExit { get; set; }

    public bool? DataProcessed { get; set; }
    public long? UnixStartTime { get; set; }


}

