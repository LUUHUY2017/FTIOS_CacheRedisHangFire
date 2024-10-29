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

    public const string Manager_Role = "Manager";
    public static readonly RoleObject Manager = new RoleObject { Module = Module.Master, Id = "ebed26a4-a972-46fe-8dfa-30b71502dd0b", Name = Manager_Role, Description = "Quản lý trường" };

    public const string Operator_Role = "Operator";
    public static readonly RoleObject Operator = new RoleObject { Module = Module.Master, Id = "2bc6badb-5b9e-48a4-b2ba-24a834d6a3c2", Name = Operator_Role, Description = "Vận hành" };
    #endregion


    #region Role

    public const string SuperAdminPage = $"{SuperAdmin_Role}";
    public const string AdminPage = $"{Admin_Role}";
    public const string ManagerPage = $"{Manager_Role}";
    public const string OperatorPage = $"{Operator_Role}";


    #endregion

    public static List<IdentityRole> IdentityRoleList()
    {
        return new List<IdentityRole>()
        {
            SuperAdmin.ToIdentityRole(),
            Admin.ToIdentityRole(),
            Manager.ToIdentityRole(),
            Operator.ToIdentityRole(),

        };
    }
}
