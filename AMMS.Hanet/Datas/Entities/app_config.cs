using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMS.Hanet.Datas.Entities;

[Table("app_config", Schema = "Hanet")]

public class app_config
{
    [Key]
    public string Id { get; set; }
    public string? ClientScret {  get; set; }
    public string? ClientId { get; set; }
    public string? PlateId { get; set; }
    public string? UserId {  get; set; }
    public string? Email {  get; set; }
    public long? Expire {  get; set; }
    public string? TokenType {  get; set; }
    public string? AccessToken {  get; set; }
    public string? RefreshToken {  get; set; }
    public string? Description { get; set; }
    public string? GrantType { get; set; }

}