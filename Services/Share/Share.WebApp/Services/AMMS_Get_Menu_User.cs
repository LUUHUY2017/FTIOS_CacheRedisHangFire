using Shared.Core.Identity.Object;

namespace AMMS.WebApp.Share.Services;
public class AMMS_Get_Menu_User
{
    public static string? Host = "";
    public static string? AccessToken = "";

    public static List<MenuShowModel> GetMenuByUser(List<PageObject> pages, List<CategoryMenu> categories, List<string>? pageIds)
    {
        var menu = new List<MenuShowModel>();
        try
        {
            var menu1 = (from r in pages
                         join _ct1 in categories on r.CategoryMenu equals _ct1.Id into C1
                         from ct1 in C1.DefaultIfEmpty()
                         select new MenuShowModel()
                         {
                             Id = r.Id,
                             Module = r.Module,
                             Name = r.Name,
                             Description = r.Description,
                             RolePermission = r.RolePermission,
                             Url = r.Url,
                             RolePermissions = r.RolePermission.Split(',').ToList(),

                             No = ct1 != null ? ct1.No : 1,
                             CategoryMenu = ct1 != null ? ct1.Id : "",
                             Icon = ct1 != null ? ct1.Icon : "",
                             Parent = ct1 != null ? ct1.Parent : "",
                             Level = ct1 != null ? ct1.Level : 1,
                             CategoryName = ct1 != null ? ct1.CategoryName : "",
                         }).ToList();
            foreach (var item in menu1)
            {
                if (item.Level == 2)
                {
                    var menuIte = categories.FirstOrDefault(o => o.Id == item.Parent);
                    if (menuIte != null)
                        item.CategoryName_1 = menuIte.CategoryName;
                }
            }

            if (pageIds != null && pageIds.Count > 0)
                menu1 = menu1.Where(p => pageIds.Contains(p.Id)).ToList();
            //else
            //    menu1 = menu1;


            // Menu cấp 1
            var menu_level1 = menu1.Where(o => o.Level == 1).GroupBy(o => o.CategoryName).Select(o => new MenuShowModel()
            {
                Icon = o.First().Icon,
                No = o.First().No,
                CategoryName = o.First().CategoryName,
                Level = o.First().Level,
                Parent = o.First().Parent,
                List = o.OrderBy(o => o.No).ToList(),
            }).OrderBy(o => o.No).ToList();

            // Menu cấp 2
            var menu_level2 = menu1.Where(o => o.Level == 2).GroupBy(o => o.CategoryName_1).Select(o => new MenuShowModel()
            {
                Icon = o.First().Icon,
                No = o.First().No,
                CategoryName = o.First().CategoryName_1,
                Level = o.First().Level,
                Parent = o.First().Parent,
                List = o.OrderBy(o => o.No).ToList(),
            }).OrderBy(o => o.No).ToList();
            foreach (var item in menu_level2)
            {
                item.List = item.List.GroupBy(o => o.CategoryName).Select(o => new MenuShowModel()
                {
                    CategoryName = o.First().CategoryName,
                    No = o.First().No,
                    Parent = o.First().Parent,
                    Level = o.First().Level,
                    Url = o.First().Url,
                    Name = o.First().Name,
                    Icon = o.First().Icon,
                    List = o.OrderBy(o => o.No).ToList(),
                }).OrderBy(o => o.No).ToList();
            }

            // Sắp xếp lại thứ tự Menu
            menu = menu.Concat(menu_level1).Concat(menu_level2).OrderBy(o => o.No).ToList();
        }
        catch (Exception ex)
        {
        }
        return menu;

    }
}
