namespace Shared.Core.Commons
{
    public abstract class BaseFilterRequest
    {
        public string? ColumnTable { get; set; } = "";
        public string? Actived { get; set; } = "1";
        public string? Deleted { get; set; } = "0";
        public string? Key { get; set; } = "";
        public string? Export { get; set; } = "0";
    }


}
