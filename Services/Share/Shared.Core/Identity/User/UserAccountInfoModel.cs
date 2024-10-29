namespace Shared.Core.Identity.User;
public class UserAccountInfoModel
{
    public int Code { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; } = "";
    public UserAccountGeneralResponse Data { get; set; }
}

public class UserAccountInfoResponse
{
    public string? Id { get; set; }
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PersonId { get; set; }
    public string? OrganizationId { get; set; }
    public string? OrganizationName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? UserType { get; set; }
}

public class UserAccountGeneralResponse
{
    public List<string?>? PageId { get; set; }
}

