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

    public static CategoryMenu QuanLy { get; private set; } = new CategoryMenu()
    {
        Id = "A9C52B55-A3BE-496F-80E4-5CDAD0BAAA19",
        Level = 1,
        CategoryName = "Quản lý",
        Parent = "",
        Icon = "ph-circles-three",
        No = 21,
    };


    public static CategoryMenu QuanLyVanTai { get; private set; } = new CategoryMenu()
    {
        Id = "08b9082a-6e65-4c98-8694-7bc074049368",
        Level = 1,
        CategoryName = "Quản lý vận tải",
        Parent = "",
        Icon = "ph-circles-three",
        No = 5,
    };
    public static CategoryMenu XepLuot { get; private set; } = new CategoryMenu()
    {
        Id = "f94270a0-a193-4a28-b9e9-e785954c3a21",
        Level = 1,
        CategoryName = "Xếp lượt",
        Parent = "",
        Icon = "ph-circles-three",
        No = 6,
    };

    public static CategoryMenu XeRaVaoCong { get; private set; } = new CategoryMenu()
    {
        Id = "0ddccf86-dc8c-4af2-8004-2bf202a0058d",
        Level = 1,
        CategoryName = "Xe ra vào cổng",
        Parent = "",
        Icon = "ph-circles-three",
        No = 7,
    };
    public static CategoryMenu CauCan { get; private set; } = new CategoryMenu()
    {
        Id = "43424d42-68f1-4fdc-9b59-2cf4498d899d",
        Level = 1,
        CategoryName = "Cầu cân",
        Parent = "",
        Icon = "ph-circles-three",
        No = 8,
    };

    public static CategoryMenu XeRaVaoKho { get; private set; } = new CategoryMenu()
    {
        Id = "a67c6bb5-4ca6-414e-a3cc-47271aa29f54",
        Level = 1,
        CategoryName = "Xe ra vào kho",
        Parent = "",
        Icon = "ph-circles-three",
        No = 9,
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
    public static CategoryMenu KhachHang { get; private set; } = new CategoryMenu()
    {
        Id = "78CC4BF2-1C04-4454-9A29-523096F59C12",
        Level = 2,
        CategoryName = "Khách hàng",
        Parent = KhaiBaoDuLieu.Id,
        Icon = "ph-users-three",
        No = 2,
    };
    public static CategoryMenu NhaCungCap { get; private set; } = new CategoryMenu()
    {
        Id = "27254F39-3EE4-424B-9A8D-34BE709E5066",
        Level = 2,
        CategoryName = "Nhà cung cấp",
        Parent = KhaiBaoDuLieu.Id,
        Icon = "ph-users-three",
        No = 3,
    };

    public static CategoryMenu DonViVanTai { get; private set; } = new CategoryMenu()
    {
        Id = "27254F39-3EE4-424B-9A8D-34BE709E5011",
        Level = 2,
        CategoryName = "Đơn vị vận tải",
        Parent = KhaiBaoDuLieu.Id,
        Icon = "ph-users-four",
        No = 4,
    };
    public static CategoryMenu Kho { get; private set; } = new CategoryMenu()
    {
        Id = "8C562811-A2BA-437D-8C1C-9BA3FDD80E0E",
        Level = 2,
        CategoryName = "Kho",
        Parent = KhaiBaoDuLieu.Id,
        Icon = "ph-gift",
        No = 5,
    };
    public static CategoryMenu HangHoa { get; private set; } = new CategoryMenu()
    {
        Id = "F713B83C-379C-4E37-B741-E1C5DF6551E9",
        Level = 2,
        CategoryName = "Hàng hóa",
        Parent = KhaiBaoDuLieu.Id,
        Icon = "ph-gift",
        No = 6,
    };
    public static CategoryMenu PhuongTien { get; private set; } = new CategoryMenu()
    {
        Id = "A1B24262-58B5-417B-BC06-25BD6769BDC5",
        Level = 2,
        CategoryName = "Phương tiện",
        Parent = KhaiBaoDuLieu.Id,
        Icon = "ph-car-simple",
        No = 7,
    };
    public static CategoryMenu The { get; private set; } = new CategoryMenu()
    {
        Id = "1C43466E-253F-4E3D-8149-EA92E51F8190",
        Level = 2,
        CategoryName = "Thẻ",
        Parent = KhaiBaoDuLieu.Id,
        Icon = "ph-identification-card",
        No = 8,
    };
    #endregion

    #endregion
    public static List<CategoryMenu> ListCategory
    {
        get
        {
            var _ListMenu = new List<CategoryMenu>();

            _ListMenu.Add(Dashboard);
            _ListMenu.Add(KhaiBaoDuLieu);
            _ListMenu.Add(TheoDoiGiamSat);
            _ListMenu.Add(QuanLy);

            _ListMenu.Add(DuLieuChung);
            _ListMenu.Add(KhachHang);
            _ListMenu.Add(NhaCungCap);
            _ListMenu.Add(DonViVanTai);
            _ListMenu.Add(Kho);
            _ListMenu.Add(HangHoa);
            _ListMenu.Add(PhuongTien);
            _ListMenu.Add(The);


            _ListMenu.Add(QuanLyVanTai);
            _ListMenu.Add(XepLuot);
            _ListMenu.Add(XeRaVaoCong);
            _ListMenu.Add(CauCan);
            _ListMenu.Add(XeRaVaoKho);
            _ListMenu.Add(ThietLap);

            return _ListMenu;
        }
    }



}
