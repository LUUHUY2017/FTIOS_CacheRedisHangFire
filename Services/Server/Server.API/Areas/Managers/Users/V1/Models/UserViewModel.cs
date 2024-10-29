namespace Server.API.Areas.Managers.Users.V1.Models;

public class UserViewModel
{
    public string? Id { get; set; }
    public string? FirstName { get; set; }
    public string? UserName { get; set; }
    public string? Password { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Descriptions { get; set; }
    public bool? Actived { get; set; }
    public string? CompanyId { get; set; }
    public string? Type {  get; set; }
}
