using Shared.Core.Identity.Object;

namespace Shared.Core.Identity.Menu;

public partial class PagesConst
{
    #region MasterData
    public static readonly PageObject URL_Dashboard = new PageObject
    {
        Id = "A2F5D3C2-955B-4D01-AAB9-97110CDF5477",
        Name = "Dashboard",
        Url = "Home",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.Dashboard.Id,
    };

    public static readonly PageObject URL_V1_CheckInByDay = new PageObject
    {
        Id = "f95d61e5-128d-4a90-aa60-a13124b32466",
        Name = "Lịch sử điểm danh",
        Url = "V1/CheckInByDay",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.BaoCao.Id,
    };

    public static readonly PageObject URL_V1_Student = new PageObject
    {
        Id = "19fbe615-372c-4deb-b18d-7c7d12ed129b",
        Name = "Danh sách học sinh SMAS",
        Url = "V1/Student",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };

    public static readonly PageObject URL_V1_SyncDeviceServer = new PageObject
    {
        Id = "a027e68b-b638-47f8-83de-7f0daedb0b45",
        Name = "Lịch sử đồng bộ học sinh",
        Url = "V1/SyncDeviceServer",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };




    public static readonly PageObject URL_V1_DeviceSuperAdmin = new PageObject
    {
        Id = "e7d6cf64-4105-44c5-8f94-ba4930db4e22",
        Name = "Khai báo thiết bị ",
        Url = "V1/Device",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };

    public static readonly PageObject URL_V1_DeviceAdmin = new PageObject
    {
        Id = "e38a9982-ef2d-407b-abfb-6b340b9633a2",
        Name = "Quản lý thiết bị ",
        Url = "V1/DeviceAdmin",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };
    public static readonly PageObject URL_V1_Organization = new PageObject
    {
        Id = "19fbe615-372c-4deb-b18d-7c7d12ed129b",
        Name = "Quản lý trường",
        Url = "V1/Organization",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };

    public static readonly PageObject URL_V1_MonitorDevice = new PageObject
    {
        Id = "257961e3-0ed6-480f-a414-ce7341818d65",
        Name = "Giám sát thiết bị",
        Url = "V1/MonitorDevice",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.TheoDoiGiamSat.Id,
    };


    public static readonly PageObject URL_V1_User = new PageObject
    {
        Id = "8232d972-703d-472a-9a2b-15d4f1c2cfab",
        Name = "Tài khoản",
        Url = "V1/User",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.QuanLyTaiKhoan.Id,
    };

    public static readonly PageObject URL_V1_RoleGroup = new PageObject
    {
        Id = "e2148949-136f-4741-9082-bb6decdfe314",
        Name = "Nhóm quyền",
        Url = "V1/RoleGroup",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.QuanLyTaiKhoan.Id,
    };

    public static readonly PageObject URL_V1_Configuration = new PageObject
    {
        Id = "8276cdb5-e7ac-4e35-9d37-1a9b0dd7442f",
        Name = "Thời gian điểm danh",
        Url = "V1/Configuration",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.ThietLap.Id,
    };

    public static readonly PageObject URL_V1_SystemConfiguration = new PageObject
    {
        Id = "7a7ad298-7ed2-44ba-a931-fa15b53b9867",
        Name = "Tích hợp VTSMAS",
        Url = "V1/SystemConfiguration",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.ThietLap.Id,
    };

    #endregion

    public static List<MenuShowModel> Menu_MD_Left = new List<MenuShowModel>();
    public static List<PageObject> _Menu_MD_Left
    {
        get
        {
            var _ListMenu = new List<PageObject>();

            _ListMenu.Add(URL_Dashboard);
            _ListMenu.Add(URL_V1_CheckInByDay);

            _ListMenu.Add(URL_V1_Student);
            _ListMenu.Add(URL_V1_SyncDeviceServer);

            _ListMenu.Add(URL_V1_DeviceSuperAdmin);
            _ListMenu.Add(URL_V1_DeviceAdmin);
            _ListMenu.Add(URL_V1_Organization);
            _ListMenu.Add(URL_V1_MonitorDevice);

            _ListMenu.Add(URL_V1_Configuration);
            _ListMenu.Add(URL_V1_SystemConfiguration);
        


            _ListMenu.Add(URL_V1_User);
            _ListMenu.Add(URL_V1_RoleGroup);

            return _ListMenu;
        }
    }

}








