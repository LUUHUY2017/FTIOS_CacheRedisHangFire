using Shared.Core.Identity.Object;

namespace Shared.Core.Identity.Menu;

public partial class PagesConst
{
    #region MasterData
    public static readonly PageObject URL_Dashboard = new PageObject
    {
        Id = "A2F5D3C2-955B-4D01-AAB9-97110CDF5477",
        Name = "Dashboard",
        Url = "V1/Dashboard",
        Module = Module.Master,
        RolePermission = RoleConst.DashboardPage,
        CategoryMenu = Category.Dashboard.Id,
    };

    public static readonly PageObject URL_Organization = new PageObject
    {
        Id = "9A0E9A36-EA87-438B-B2D6-A2906B5B4C52",
        Url = "V1/Organization",
        Name = "Tổ chức",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.DuLieuChung.Id,
    };

    public static readonly PageObject URL_Person = new PageObject
    {
        Id = "EA9EE897-AE38-4E05-A684-2DAAA1F4D382",
        Name = "Người",
        Url = "V1/Person",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.DuLieuChung.Id,
    };

    public static readonly PageObject URL_Customer = new PageObject
    {
        Id = "B3501C1D-8F0F-4CB3-8C9F-7934BA54649F",
        Name = "Khách hàng",
        Url = "V1/Customer",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhachHang.Id,
    };
    public static readonly PageObject URL_CustomerStaff = new PageObject
    {
        Id = "ADB183CC-68D1-416D-AD03-5965C005CD52",
        Url = "V1/CustomerStaff",
        Name = "Nhân sự khách hàng",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhachHang.Id,
    };
    public static readonly PageObject URL_CustomerVehicle = new PageObject
    {
        Id = "68F1D741-2F79-45DD-81C3-D6C20545022A",
        Url = "V1/CustomerVehicle",
        Name = "Phương tiện khách hàng",
        RolePermission = RoleConst.MasterDataPage,
        Module = Module.Master,
        CategoryMenu = Category.KhachHang.Id,
    };

    public static readonly PageObject URL_Vendor = new PageObject
    {
        Id = "DC791D7C-0D0C-4FA2-ADCF-6E606603B011",
        Url = "V1/Vendor",
        Name = "Nhà cung cấp",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.NhaCungCap.Id,
    };

    public static readonly PageObject URL_VendorStaff = new PageObject
    {
        Id = "DC791D7C-0D0C-4FA2-ADCF-6E606603B096",
        Name = "Nhân sự nhà cung cấp",
        Url = "V1/VendorStaf",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.NhaCungCap.Id,
    };

    public static readonly PageObject URL_TransportUnit = new PageObject
    {
        Id = "f4395af5-c165-4310-ba9e-1fb7f5d57aef",
        Name = "Nhà vận tải",
        Url = "V1/Transportunit",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.DonViVanTai.Id,
    };

    public static readonly PageObject URL_VendorVehicle = new PageObject
    {
        Id = "288591B7-C1E4-4D4F-9F5A-D5D9222AA777",
        Name = "Phương tiện nhà cung cấp",
        Url = "V1/VendorVehicle",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.NhaCungCap.Id,
    };

    public static readonly PageObject URL_WarehouseCategory = new PageObject
    {
        Id = "46D07C78-EF68-4F5F-A72D-CD7BAFBCB5A6",
        Name = "Danh mục kho",
        Url = "V1/WarehouseCategory",
        RolePermission = RoleConst.MasterDataPage,
        Module = Module.Master,
        CategoryMenu = Category.Kho.Id,
    };
    public static readonly PageObject URL_Warehouse = new PageObject
    {
        Id = "71AAFAE3-B460-4C19-B2C0-B24831EC27A2",
        Url = "V1/Warehouse",
        Name = "Kho",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.Kho.Id,
    };

    public static readonly PageObject URL_ItemCategory = new PageObject
    {
        Id = "EF6DFC08-7150-4113-B9C6-29AD5DC73BBD",
        Url = "V1/ItemCategory",
        Name = "Danh mục hàng hóa",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.HangHoa.Id,
    };
    public static readonly PageObject URL_Item = new PageObject
    {
        Id = "0AE2041C-9333-4AA8-8254-61BE8147FF89",
        Url = "V1/Item",
        Name = "Hàng hóa",
        RolePermission = RoleConst.MasterDataPage,
        Module = Module.Master,
        CategoryMenu = Category.HangHoa.Id,
    };

    public static readonly PageObject URL_VehicleCategory = new PageObject
    {
        Id = "660864C5-AF6A-46D2-B31E-CCF50992E983",
        Name = "Danh mục phương tiện",
        Url = "V1/VehicleCategory",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.PhuongTien.Id,
    };
    public static readonly PageObject URL_Vehicle = new PageObject
    {
        Id = "F64431B5-8B99-4F76-BBDC-E4D2D6CAE372",
        Name = "Phương tiện",
        Url = "V1/Vehicle",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.PhuongTien.Id,
    };

    public static readonly PageObject URL_CardCategory = new PageObject
    {
        Id = "779669B2-2798-4810-979F-F8CBDAACB05D",
        Url = "V1/CardCategory",
        Name = "Danh mục thẻ",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.The.Id,
    };
    public static readonly PageObject URL_Card = new PageObject
    {
        Id = "744D2EF6-7073-4819-9DBA-B1F77B2EE405",
        Url = "V1/Card",
        Name = "Thẻ",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.The.Id,
    };

    public static readonly PageObject URL_SystemMonitor = new PageObject
    {
        Id = "0A20DE50-FADA-42BA-85B9-48EA7745A061",
        Name = "Hệ thống",
        Url = "V1/SystemMonitor",
        Module = Module.Master,
        RolePermission = RoleConst.AdminPage,
        CategoryMenu = Category.TheoDoiGiamSat.Id,
    };


    public static readonly PageObject URL_DeviceMonitor = new PageObject
    {
        Id = "4CB56ABD-4066-4632-AC58-1F6522DA9B3E",
        Name = "Thiết bị",
        Url = "V1/DeviceMonitor",
        Module = Module.Master,
        RolePermission = RoleConst.AdminPage,
        CategoryMenu = Category.TheoDoiGiamSat.Id,
    };

    public static readonly PageObject URL_RoleGroup = new PageObject
    {
        Id = "1b9e89e2-d726-445d-883c-9d9559be9944",
        Name = "Nhóm quyền",
        Url = "V1/RoleGroup",
        Module = Module.Master,
        RolePermission = RoleConst.AdminPage,
        CategoryMenu = Category.QuanLy.Id,
    };

    public static readonly PageObject URL_User = new PageObject
    {
        Id = "70856f21-210b-4be7-be56-e77796824c27",
        Url = "V1/User",
        Name = "Người dùng",
        Module = Module.Master,
        RolePermission = RoleConst.AdminPage,
        CategoryMenu = Category.QuanLy.Id,
    };


    public static readonly PageObject URL_Client = new PageObject
    {
        Id = "02fc6ec0-f526-4631-a602-7dfecf1e2782",
        Url = "V1/Client",
        Name = "Ứng dụng",
        Module = Module.Master,
        RolePermission = RoleConst.AdminPage,
        CategoryMenu = Category.QuanLy.Id,
    };
    #endregion

    public static List<MenuShowModel> Menu_MD_Left = new List<MenuShowModel>();
    public static List<PageObject> _Menu_MD_Left
    {
        get
        {
            var _ListMenu = new List<PageObject>();

            _ListMenu.Add(URL_Dashboard);
            _ListMenu.Add(URL_Organization);
            _ListMenu.Add(URL_Person);


            _ListMenu.Add(URL_Customer);
            _ListMenu.Add(URL_CustomerStaff);
            _ListMenu.Add(URL_CustomerVehicle);

            _ListMenu.Add(URL_Vendor);
            _ListMenu.Add(URL_VendorStaff);
            _ListMenu.Add(URL_VendorVehicle);

            _ListMenu.Add(URL_TransportUnit);

            _ListMenu.Add(URL_WarehouseCategory);
            _ListMenu.Add(URL_Warehouse);

            _ListMenu.Add(URL_ItemCategory);
            _ListMenu.Add(URL_Item);

            _ListMenu.Add(URL_VehicleCategory);
            _ListMenu.Add(URL_Vehicle);

            _ListMenu.Add(URL_CardCategory);
            _ListMenu.Add(URL_Card);

            _ListMenu.Add(URL_SystemMonitor);
            _ListMenu.Add(URL_DeviceMonitor);

            _ListMenu.Add(URL_RoleGroup);
            _ListMenu.Add(URL_User);
            _ListMenu.Add(URL_Client);

            return _ListMenu;
        }
    }

}








