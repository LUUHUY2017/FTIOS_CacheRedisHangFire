namespace Server.Application.MasterDatas.A2.Devices.Models.Commons;

public static class DeviceBrandConst
{
    public const string Hanet = "hanet";
    public const string ZKTeco = "zkteco";
    public static readonly List<object> BrandNames = new List<object>
    {
        new { Id = Hanet, Name = "Hanet" },
        new { Id = ZKTeco, Name = "ZKTeco" }
    };
}
