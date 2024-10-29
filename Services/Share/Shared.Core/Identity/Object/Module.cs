namespace Shared.Core.Identity.Object;

public class Module
{
    public const string Master = "Master";
    public const string GateIO = "GateIO";
    public const string Delivery = "Delivery";
    public const string WMS = "WMS";
    public const string TA = "TA";
    public const string CT = "CT";
}


public partial class Category
{
    #region MD

    #region level1
    public static CategoryMenu Dashboard { get; private set; } = new CategoryMenu()
    {
        Id = "E1C153D6-3D8D-40C3-9A4F-C26026457DFA",
        Level = 1,
        CategoryName = "Dashboard",
        Parent = "",
        Icon = "ph-house",
        No = 1,
    };

    public static CategoryMenu BaoCao { get; private set; } = new CategoryMenu()
    {
        Id = "96550835-70b4-4d67-a2ac-60dfd1d97228",
        Level = 1,
        CategoryName = "Báo cáo",
        Parent = "",
        Icon = "ph-desktop-tower",
        No = 10,
    };
    public static CategoryMenu KhaiBaoDuLieu { get; private set; } = new CategoryMenu()
    {
        Id = "C1CB91B5-3FE4-48F9-9505-D28194BBB9A1",
        Level = 1,
        CategoryName = "Khai báo dữ liệu",
        Parent = "",
        Icon = "ph-wrench",
        No = 17,
    };
    public static CategoryMenu TheoDoiGiamSat { get; private set; } = new CategoryMenu()
    {
        Id = "95704D99-AA3D-48CA-B1BA-F627262B73AD",
        Level = 1,
        CategoryName = "Theo dõi giám sát",
        Parent = "",
        Icon = "ph-cloud-arrow-up",
        No = 20,
    };

    public static CategoryMenu QuanLyTaiKhoan { get; private set; } = new CategoryMenu()
    {
        Id = "A9C52B55-A3BE-496F-80E4-5CDAD0BAAA19",
        Level = 1,
        CategoryName = "Quản lý",
        Parent = "",
        Icon = "ph-circles-three",
        No = 21,
    };


    public static CategoryMenu ThietLap { get; private set; } = new CategoryMenu()
    {
        Id = "3883f71a-ccc7-46a4-bd44-c278f1a46dd4",
        Level = 1,
        CategoryName = "Thiết lập",
        Parent = "",
        Icon = "ph-circles-three",
        No = 18,
    };


    #endregion


    #region level2
    public static CategoryMenu DuLieuChung { get; private set; } = new CategoryMenu()
    {
        Id = "57F3E87A-B0C0-4BD1-9DEF-3A0A259B208D",
        Level = 2,
        CategoryName = "Dữ liệu chung",
        Parent = KhaiBaoDuLieu.Id,
        Icon = "ph-users",
        No = 1,
    };
    #endregion

    #endregion
    public static List<CategoryMenu> ListCategory
    {
        get
        {
            var _ListMenu = new List<CategoryMenu>();

            _ListMenu.Add(Dashboard);
            _ListMenu.Add(BaoCao);
            _ListMenu.Add(KhaiBaoDuLieu);
            _ListMenu.Add(TheoDoiGiamSat);
            _ListMenu.Add(QuanLyTaiKhoan);

            _ListMenu.Add(DuLieuChung);
            _ListMenu.Add(ThietLap);

            return _ListMenu;
        }
    }



}
