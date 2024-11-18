namespace Server.Application.MasterDatas.A2.DashBoards.V1.Models;

public class DashBoardFilter
{
    public string? ColumnTable { get; set; } = null;
    public string? Key { get; set; } = null;
    public DateTime? TimeFilter { get; set; } = null!;

}
