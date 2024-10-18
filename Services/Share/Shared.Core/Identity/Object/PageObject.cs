namespace Shared.Core.Identity.Object;

public class PageObject
{
    public string? Id { get; set; }
    public string? Module { get; set; }
    public string? RolePermission { get; set; }
    public string? Url { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? CategoryMenu { get; set; }
}

public class CategoryMenu
{
    public string Id { get; set; }
    public int Level { get; set; }
    public int No { get; set; }
    public string? CategoryName { get; set; }
    public string? Parent { get; set; }
    public string? Icon { get; set; }
}

public class MenuShowModel : PageObject
{
    public int? No { get; set; }
    public string? Icon { get; set; }
    public string? Parent { get; set; }
    public int? Level { get; set; }
    public string? CategoryName { get; set; }
    public string? CategoryName_1 { get; set; }
    public List<string>? RolePermissions { get; set; }
    public List<MenuShowModel>? List { get; set; }

}
