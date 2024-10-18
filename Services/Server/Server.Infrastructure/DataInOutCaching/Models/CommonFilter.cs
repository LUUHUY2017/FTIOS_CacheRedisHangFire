namespace Server.Infrastructure.DataInOutCaching.Models
{
    public class CommonFilter
    {
        public string? Key { get; set; } = "";
        public string? ColumnTable { get; set; } = "";

        public bool? Actived { get; set; } = true;
    }
}
