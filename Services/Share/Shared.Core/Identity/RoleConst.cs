using Microsoft.AspNetCore.Identity;
using Shared.Core.Identity.Object;

namespace Shared.Core.Identity;

public static class RoleConst
{

    #region Quản trị
    public const string SuperAdmin_Role = "SuperAdmin";
    public static readonly RoleObject SuperAdmin = new RoleObject { Module = Module.Master, Id = "cfe0acd3-ccf3-4986-9b48-508355193039", Name = SuperAdmin_Role, Description = "Khởi tạo hệ thống" };

    public const string Admin_Role = "Admin";
    public static readonly RoleObject Admin = new RoleObject { Module = Module.Master, Id = "ea7da92e-f11c-45a7-aa4f-24761f0c4f93", Name = Admin_Role, Description = "Quản trị hệ thống - có toàn quyền trên tất cả các chức năng" };

    public const string MasterData_Role = "MasterData";
    public static readonly RoleObject MasterData = new RoleObject { Module = Module.Master, Id = "ebed26a4-a972-46fe-8dfa-30b71502dd0b", Name = MasterData_Role, Description = "Quản lý master data" };

    public const string Role_Role = "RoleAdmin";
    public static readonly RoleObject RoleAdmin = new RoleObject { Module = Module.Master, Id = "2bc6badb-5b9e-48a4-b2ba-24a834d6a3c2", Name = Role_Role, Description = "Phân quyền hệ thống" };

    public const string UserAdmin_Role = "UserAdmin";
    public static readonly RoleObject UserAdmin = new RoleObject { Module = Module.Master, Id = "bd8af76f-d66e-4f48-a4ed-642a08319170", Name = UserAdmin_Role, Description = "Quản lý tài khoản" };
    public const string Role_Manage = $"{Admin_Role}, {Role_Role}";
    #endregion


    #region Delivery
    public const string DeliveryAdmin_Role = "DeliveryAdmin";
    public static readonly RoleObject DeliveryAdmin = new RoleObject { Module = Module.Delivery, Id = "e2f0089a-d9d5-4667-b528-99317c462bf0", Name = DeliveryAdmin_Role, Description = "Quản trị hệ thống - nhập xuất hàng" };

    public const string DeliveryManage_role = "DeliveryManage";
    public static readonly RoleObject DeliveryManage = new RoleObject { Module = Module.Delivery, Id = "bb0db877-bac2-434c-93df-25585e4fb7d2", Name = DeliveryManage_role, Description = "Quản lý hệ thống - nhập xuất hàng" };

    public const string DeliveryApprover_Role = "DeliveryApprover";
    public static readonly RoleObject DeliveryApprover = new RoleObject { Module = Module.Delivery, Id = "c02e55d4-9dbf-4585-bca2-9cab8b2024c9", Name = DeliveryApprover_Role, Description = "Phê duyệt kế hoạch nhập xuất hàng" };

    public const string DeliveryOperateTheScale_Role = "DeliveryOperateTheScale";
    public static readonly RoleObject DeliveryOperateTheScale = new RoleObject { Module = Module.Delivery, Id = "08b0340c-d520-4cee-b2fd-332c2442e14a", Name = DeliveryOperateTheScale_Role, Description = "Vận hành cân" };

    public const string DeliveryReport_Role = "DeliveryReport";
    public static readonly RoleObject DeliveryReport = new RoleObject { Module = Module.Delivery, Id = "facaca76-8b53-4db2-a3e7-db0d07729559", Name = DeliveryReport_Role, Description = "Báo cáo" };


    public const string DeliveryCustomer_Role = "DeliveryCustomer";
    public static readonly RoleObject DeliveryCustomerRoles = new RoleObject { Module = Module.Delivery, Id = "29cf73bc-3528-473b-97d9-aaf559ea8fb3", Name = DeliveryCustomer_Role, Description = "Khách hàng / Nhà phân phối" };


    public const string DeliveryVendor_Role = "DeliveryVendor";
    public static readonly RoleObject DeliveryVendorRoles = new RoleObject { Module = Module.Delivery, Id = "3572e1e1-9ddd-4340-b9dd-87ad2f2d1d68", Name = DeliveryVendor_Role, Description = "Nhà cung cấp" };


    public const string DeliveryTransportUnit_Role = "DeliveryTransportUnit";
    public static readonly RoleObject DeliveryTransportUnitRoles = new RoleObject { Module = Module.Delivery, Id = "66ee2a0f-397e-4c8f-a293-08a859f3b997", Name = DeliveryTransportUnit_Role, Description = "Đơn vị vận tải" };


    public const string TruckDriver_Role = "TruckDriver";
    public static readonly RoleObject TruckDriver = new RoleObject { Module = Module.Delivery, Id = "7c31a1a7-7a62-4aca-b55a-cb99ca7075af", Name = TruckDriver_Role, Description = "Cổng thông tin và app mobile của lái xe nhập xuất hàng" };

    public const string DeliveryOrder_Role = "DeliveryOrder";
    public static readonly RoleObject DeliveryOrder = new RoleObject { Module = Module.Delivery, Id = "E55B1716-A52B-4E61-82C5-C145677BC3B8", Name = DeliveryOrder_Role, Description = "Quản lý đơn hàng" };

    #endregion


    #region Role

    public const string DashboardPage = $"{SuperAdmin_Role},{DeliveryAdmin_Role}";
    public const string AdminPage = $"{SuperAdmin_Role},{Admin_Role}";
    public const string MasterDataPage = $"{SuperAdmin_Role},{MasterData_Role}";

    public const string TransportUnitPage = $"{SuperAdmin_Role},{DeliveryTransportUnit_Role}";
    public const string DeliveryVendorPage = $"{SuperAdmin_Role},{DeliveryVendor_Role}";
    public const string DeliveryOrderPage = $"{SuperAdmin_Role},{DeliveryVendor_Role}";
    public const string DeliveryCustomerPage = $"{SuperAdmin_Role},{DeliveryCustomer_Role}";
    public const string DeliveryTransportUnitPage = $"{SuperAdmin_Role},{DeliveryTransportUnit_Role}";
    public const string TruckDriverPage = $"{SuperAdmin_Role},{TruckDriver_Role}";


    #endregion

    public static List<IdentityRole> IdentityRoleList()
    {
        return new List<IdentityRole>()
        {
            SuperAdmin.ToIdentityRole(),
            Admin.ToIdentityRole(),
            MasterData.ToIdentityRole(),
            RoleAdmin.ToIdentityRole(),
            UserAdmin.ToIdentityRole(),


            DeliveryAdmin.ToIdentityRole(),
            DeliveryManage.ToIdentityRole(),
            DeliveryApprover.ToIdentityRole(),
            DeliveryOperateTheScale.ToIdentityRole(),
            DeliveryReport.ToIdentityRole(),
            DeliveryCustomerRoles.ToIdentityRole(),
            DeliveryVendorRoles.ToIdentityRole(),
            DeliveryTransportUnitRoles.ToIdentityRole(),
            TruckDriver.ToIdentityRole(),
            DeliveryOrder.ToIdentityRole(),

        };
    }
}