namespace Server.API.APIs.Reports.Dashboards.V1.Responses
{
    public class ObjectDashboard
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

    public class ObjectDataDashboard : ObjectDashboard
    {
        public float Percent { get; set; }
    }
}
