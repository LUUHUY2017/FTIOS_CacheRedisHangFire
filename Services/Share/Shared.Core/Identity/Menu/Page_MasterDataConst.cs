using Shared.Core.Identity.Object;

namespace Shared.Core.Identity.Menu;

public partial class PagesConst
{
    #region MasterData
    public static readonly PageObject URL_Dashboard = new PageObject
    {
        Id = "A2F5D3C2-955B-4D01-AAB9-97110CDF5477",
        Name = "Dashboard",
        Url = "V1/DashBoard",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.Dashboard.Id,
    };

    public static readonly PageObject URL_V1_TimeAttenceEvent = new PageObject
    {
        Id = "f95d61e5-128d-4a90-aa60-a13124b32466",
        Name = "Lịch sử điểm danh",
        Url = "V1/TimeAttenceEvent",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.BaoCao.Id,
    };

    public static readonly PageObject URL_V1_TimeAttendenceSync = new PageObject
    {
        Id = "5ed5219f-8331-4199-b5b0-c2f7681fcea5",
        Name = "Lịch sử đồng bộ điểm danh",
        Url = "V1/TimeAttendenceSync",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.BaoCao.Id,
    };

    public static readonly PageObject URL_V1_StudentSmas = new PageObject
    {
        Id = "19fbe615-372c-4deb-b18d-7c7d12ed129b",
        Name = "Danh sách học sinh SMAS",
        Url = "V1/StudentSmas",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };

    public static readonly PageObject URL_V1_Student = new PageObject
    {
        Id = "fc531bfa-1863-4328-8688-3104ee6cd52d",
        Name = "Khai báo khuôn mặt",
        Url = "V1/Student",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };

    public static readonly PageObject URL_V1_ClassRoom = new PageObject
    {
        Id = "d007301c-f2fc-401e-a833-781f272dcfa9",
        Name = "Khối lớp",
        Url = "V1/ClassRoom",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };

    public static readonly PageObject URL_V1_SchoolYear = new PageObject
    {
        Id = "21b63ecf-aa05-441a-81ed-7fab4a3c516a",
        Name = "Năm học",
        Url = "V1/SchoolYear",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };

    public static readonly PageObject URL_V1_ClassRoomYear = new PageObject
    {
        Id = "7084de58-1d64-4db7-ac86-2e584a1780cd",
        Name = "Lớp ",
        Url = "V1/ClassRoomYear",
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


    public static readonly PageObject URL_V1_ScheduleJob = new PageObject
    {
        Id = "d2d6cf84-4105-44c5-8f94-ba4930db4e22",
        Name = "Lập lịch đồng bộ ",
        Url = "V1/ScheduleJob",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.ThietLap.Id,
    };

    public static readonly PageObject URL_V1_ScheduleReport = new PageObject
    {
        Id = "8953cf90-7f3f-4155-8d07-f80579719b33",
        Name = "Lập lịch báo cáo ",
        Url = "V1/ScheduleReport",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.ThietLap.Id,
    };

    public static readonly PageObject URL_V1_DeviceSuperAdmin = new PageObject
    {
        Id = "e7d6cf64-4105-44c5-8f94-ba4930db4e22",
        Name = "Khai báo thiết bị ",
        Url = "V1/Device",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.ThietLap.Id,
    };

    public static readonly PageObject URL_V1_DeviceAdmin = new PageObject
    {
        Id = "e38a9982-ef2d-407b-abfb-6b340b9633a2",
        Name = "Quản lý thiết bị ",
        Url = "V1/DeviceAdmin",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.ThietLap.Id,
    };

    public static readonly PageObject URL_V1_Organization = new PageObject
    {
        Id = "19fbe615-372c-4deb-b18d-7c7d12ed129b",
        Name = "Quản lý trường",
        Url = "V1/Organization",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.ThietLap.Id,
    };

    public static readonly PageObject URL_V1_OrganizationAdmin = new PageObject
    {
        Id = "9614c34d-e84e-4434-a834-42e966f39d5d",
        Name = "Trường học",
        Url = "V1/OrganizationAdmin",
        Module = Module.Master,
        RolePermission = RoleConst.SuperAdmin_Role,
        CategoryMenu = Category.ThietLap.Id,
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

    public static readonly PageObject URL_V1_MonitorAutoReport = new PageObject
    {
        Id = "5a1b7e0b-1cdc-4224-bb05-b36cd770606f",
        Name = "Lịch sử gửi email",
        Url = "V1/AutoReportMonitor",
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

    //public static readonly PageObject URL_V1_SystemConfiguration = new PageObject
    //{
    //    Id = "7a7ad298-7ed2-44ba-a931-fa15b53b9867",
    //    Name = "Tích hợp VTSMAS",
    //    Url = "V1/SystemConfiguration",
    //    Module = Module.Master,
    //    RolePermission = RoleConst.SuperAdmin_Role,
    //    CategoryMenu = Category.ThietLap.Id,
    //};

    #endregion

    public static List<MenuShowModel> Menu_MD_Left = new List<MenuShowModel>();
    public static List<PageObject> _Menu_MD_Left
    {
        get
        {
            var _ListMenu = new List<PageObject>();

            _ListMenu.Add(URL_Dashboard);
            _ListMenu.Add(URL_V1_TimeAttenceEvent);
            _ListMenu.Add(URL_V1_TimeAttendenceSync);

            //_ListMenu.Add(URL_V1_StudentSmas);
            _ListMenu.Add(URL_V1_Student);
            _ListMenu.Add(URL_V1_SyncDeviceServer);

            _ListMenu.Add(URL_V1_ClassRoom);
            _ListMenu.Add(URL_V1_SchoolYear);
            _ListMenu.Add(URL_V1_ClassRoomYear);

            _ListMenu.Add(URL_V1_ScheduleJob);
            _ListMenu.Add(URL_V1_ScheduleReport);
            _ListMenu.Add(URL_V1_DeviceSuperAdmin);
            _ListMenu.Add(URL_V1_DeviceAdmin);
            _ListMenu.Add(URL_V1_Organization);
            _ListMenu.Add(URL_V1_OrganizationAdmin);
            _ListMenu.Add(URL_V1_MonitorDevice);
            _ListMenu.Add(URL_V1_MonitorAutoReport);

            _ListMenu.Add(URL_V1_Configuration);
            //_ListMenu.Add(URL_V1_SystemConfiguration);

            _ListMenu.Add(URL_V1_User);
            _ListMenu.Add(URL_V1_RoleGroup);

            return _ListMenu;
        }
    }

}








