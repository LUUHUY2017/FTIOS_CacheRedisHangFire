using System.ComponentModel.DataAnnotations;

namespace Server.Core.Identity.Interfaces.Accounts.Requests;

public class AuthenticationRequest
{
    [Required(ErrorMessage = "Phải nhập tên sử dụng")]
    [DataType(DataType.Text)]
    [Display(Name = "Tên đăng nhập")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Phải nhập mật khẩu")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string Password { get; set; }
    public string IpAddress { get; set; } = "";
    public string DeviceType { get; set; } = "";
}


public class LoginViewModel : AuthenticationRequest
{
    //[Required(ErrorMessage = "Phải nhập tên sử dụng")]
    //[DataType(DataType.Text)]
    //[Display(Name = "Tên đăng nhập")]
    //public string UserName { get; set; }

    //[Required(ErrorMessage = "Phải nhập mật khẩu")]
    //[DataType(DataType.Password)]
    //[Display(Name = "Mật khẩu")]
    //public string? Password { get; set; }

    [Display(Name = "Ghi nhớ mật khẩu!")]
    public bool RememberMe { get; set; }

    [DataType(DataType.Url)]
    public string? ReturnUrl { get; set; }
}

public class AuthenticationMobileRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string FirebaseToken { get; set; }
    public string DeviceId { get; set; }
    public string DeviceName { get; set; }
    public string DeviceModel { get; set; }
    public string DeviceManufacturer { get; set; }
    public string DeviceVersion { get; set; }
    public string DevicePlatform { get; set; }
    public string DeviceType { get; set; }
}
