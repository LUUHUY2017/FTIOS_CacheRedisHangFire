namespace Server.Core.Identity.Interfaces.Accounts.Responses;

public class AccountResponse
{
    public string? Id { get; set; }
    public bool? Actived { get; set; } = true;
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string CitizenId { get; set; } = "";
    public string Avata { get; set; } = "";
    public string Note { get; set; } = "";
    public string? OrganizationId { get; set; } = "";
}