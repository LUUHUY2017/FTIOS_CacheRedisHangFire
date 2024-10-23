using Shared.Core.Identity.Object;

namespace Shared.Core.Identity.Menu;

public partial class PagesConst
{
    #region MasterData
    public static readonly PageObject URL_Dashboard = new PageObject
    {
        Id = "A2F5D3C2-955B-4D01-AAB9-97110CDF5477",
        Name = "Dashboard",
        Url = "/",
        Module = Module.Master,
        RolePermission = RoleConst.DashboardPage,
        CategoryMenu = Category.Dashboard.Id,
    };

    #endregion

    public static List<MenuShowModel> Menu_MD_Left = new List<MenuShowModel>();
    public static List<PageObject> _Menu_MD_Left
    {
        get
        {
            var _ListMenu = new List<PageObject>();

            _ListMenu.Add(URL_Dashboard);

            return _ListMenu;
        }
    }

}








