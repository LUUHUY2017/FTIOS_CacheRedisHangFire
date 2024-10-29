namespace Server.API.APIs.Data.Users.V1.Commons;

public static class UserTypeConst
{
    //public const string SupperAdmin = "SupperAdmin";
    public const string System = "System";
    public const string User = "User";

    public static readonly List<object> UserTypes = new List<object>
    {
        new { Id = System, Name = "Tài khoản hệ thống" },
        new { Id = User, Name = "Tài khoản trường" }
    };
}
