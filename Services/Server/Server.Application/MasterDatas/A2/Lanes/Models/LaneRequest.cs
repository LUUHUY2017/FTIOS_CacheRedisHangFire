namespace Server.Application.MasterDatas.A2.Lanes.Models
{

    public class LaneRequest
    {
        public string? Id { get; set; }
        public bool? Actived { get; set; }

        public string? OrganizationId { get; set; }
        public string? GateId { get; set; }

        public string? LaneCode { get; set; }
        public string? LaneName { get; set; }
        public string? Description { get; set; }


    }
}
