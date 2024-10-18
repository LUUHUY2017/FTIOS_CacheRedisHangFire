using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace AMMS.WebAPI.Areas.Manage.Accounts.Models;

public class LoginViewModel
{
    [Required(ErrorMessage = "Phải nhập tên sử dụng")]
    [DataType(DataType.Text)]
    [Display(Name = "Tên đăng nhập")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Phải nhập mật khẩu")]
    [DataType(DataType.Password)]
    [Display(Name = "Mật khẩu")]
    public string? Password { get; set; }

    [Display(Name = "Ghi nhớ mật khẩu!")]
    public bool RememberMe { get; set; }

    [DataType(DataType.Url)]
    public string? ReturnUrl { get; set; }
}
