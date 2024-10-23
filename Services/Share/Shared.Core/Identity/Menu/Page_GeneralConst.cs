using Shared.Core.Identity.Object;

namespace Shared.Core.Identity.Menu;

public partial class PagesConst
{
    public static List<MenuShowModel> Menu_General_Left;
    public static List<PageObject> _Menu_General_Left
    {
        get
        {
            var _ListMenu = new List<PageObject>();
            _ListMenu.AddRange(_Menu_MD_Left);

            return _ListMenu;
        }
    }

}


