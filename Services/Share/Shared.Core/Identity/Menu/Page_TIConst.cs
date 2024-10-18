using Shared.Core.Identity.Object;

namespace Shared.Core.Identity.Menu;

public partial class PagesConst
{
    #region ID
    public static readonly PageObject TI_Home = new PageObject
    {
        Id = "c8fe902e-cfb7-4cee-8d8f-11759aa111b1",
        Name = "Dashboard",
        Url = "V1/TI_Home",
        Module = Module.Delivery,
        RolePermission = RoleConst.DashboardPage,
        CategoryMenu = Category.Dashboard.Id,
    };

    public static readonly PageObject TI_Contract = new PageObject
    {
        Id = "9eedb0f1-9845-4d3a-82a3-cee5040a45d2",
        Url = "TI/v1/Contract",
        Name = "Hợp đồng",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.QuanLyVanTai.Id,
    };

    public static readonly PageObject TI_SalesOrder = new PageObject
    {
        Id = "6e5a0cad-2096-4ccb-9caf-c8784a1f05dc",
        Name = "Đơn hàng",
        Url = "TI/v1/SalesOrder",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.QuanLyVanTai.Id,
    };

    public static readonly PageObject TI_DeliveryOrder = new PageObject
    {
        Id = "ef485b9a-c161-43ea-b5a3-6ebbc6aef634",
        Name = "Kế hoạch vận chuyển",
        Url = "TI/v1/DeliveryOrder",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.QuanLyVanTai.Id,
    };
    public static readonly PageObject TI_Trip = new PageObject
    {
        Id = "42ac4ef3-913b-494e-882e-cad7465a8c0e",
        Url = "TI/v1/Trip",
        Name = "Chuyến xe",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.QuanLyVanTai.Id,
    };
    public static readonly PageObject TI_Shipment = new PageObject
    {
        Id = "f4599f5b-01ab-492e-8fc4-90752927c167",
        Url = "TI/v1/Shipment",
        Name = "Vận đơn",
        RolePermission = RoleConst.MasterDataPage,
        Module = Module.Master,
        CategoryMenu = Category.QuanLyVanTai.Id,
    };

    public static readonly PageObject TI_CheckIn = new PageObject
    {
        Id = "d29bafc3-35ef-476f-860c-b0fdbbdb4984",
        Url = "TI/V1/Checkin",
        Name = "Đăng tài",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.XepLuot.Id,
    };
    public static readonly PageObject TI_Queue = new PageObject
    {
        Id = "5d1dbf47-821d-4cbe-b37c-c49b3065d8d4",
        Name = "Xếp tài",
        Url = "TI/V1/Queue",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.XepLuot.Id,
    };
    public static readonly PageObject TI_GateGeneralReport = new PageObject
    {
        Id = "4ddbb937-de50-4bf0-8b4f-8b5d50b1effc",
        Name = "Báo cáo",
        Url = "TI/v1/GateIO/GeneralReport",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.XeRaVaoCong.Id,
    };

    public static readonly PageObject TI_WeWeighInReport = new PageObject
    {
        Id = "285e9f4f-9e67-40a1-b438-d8321d59d1f3",
        Name = "Báo cáo cân vào",
        Url = "TI/v1/WeighVoucher/WeighInReport",
        RolePermission = RoleConst.MasterDataPage,
        Module = Module.Master,
        CategoryMenu = Category.CauCan.Id,
    };
    public static readonly PageObject TI_WeGeneralReport = new PageObject
    {
        Id = "61b7577a-912f-453e-b82f-abf422a33074",
        Url = "TI/v1/WeighVoucher/GeneralReport",
        Name = "Báo cáo tổng hợp",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.CauCan.Id,
    };

    public static readonly PageObject TI_WaGeneralReport = new PageObject
    {
        Id = "215c324c-d050-441e-9dab-ca536113c943",
        Url = "TI/v1/Warehouse/GeneralReport",
        Name = "Báo cáo",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.XeRaVaoKho.Id,
    };

    public static readonly PageObject TI_TransportUnit = new PageObject
    {
        Id = "192b3059-75c6-4f94-94a1-b6f9f8bc5427",
        Name = "Nhà vận tải",
        Url = "TI/v1/TransportUnit",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };

    public static readonly PageObject TI_Customer = new PageObject
    {
        Id = "cf8126af-2ae6-4e66-afd1-147471ec478a",
        Name = "Khách hàng",
        Url = "TI/v1/Customer",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };
    public static readonly PageObject TI_Item = new PageObject
    {
        Id = "ed5cf1ad-d48c-46cd-a293-9e5c5e78cef5",
        Url = "TI/v1/Item",
        Name = "Hàng hóa",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };
    public static readonly PageObject TI_ItemCategory = new PageObject
    {
        Id = "b6f7232f-2893-4b51-bc9e-96fa0d98b7bc",
        Url = "TI/v1/ItemCategory",
        Name = "Danh mục hàng hóa",
        RolePermission = RoleConst.MasterDataPage,
        Module = Module.Master,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };

    public static readonly PageObject TI_Warehouse = new PageObject
    {
        Id = "10be4a71-47c1-4e02-ad9b-12d309ed571c",
        Url = "TI/v1/Warehouse",
        Name = "Kho",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };
    public static readonly PageObject TI_Location = new PageObject
    {
        Id = "3c44e20e-7d0d-4496-ae73-b347a23d7a35",
        Name = "Địa điểm",
        Url = "TI/v1/Location",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };
    public static readonly PageObject TI_WeighBridge = new PageObject
    {
        Id = "503a4f96-2bfc-4a51-98be-1c7c6e048581",
        Name = "Cầu cân",
        Url = "TI/v1/WeighBridge",
        Module = Module.Master,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.ThietLap.Id,
    };
    #endregion

    public static List<MenuShowModel> Menu_TI_Left = new List<MenuShowModel>();
    public static List<PageObject> _Menu_TI_Left
    {
        get
        {
            var _ListMenu = new List<PageObject>();

            _ListMenu.Add(TI_Home);
            _ListMenu.Add(TI_Contract);
            _ListMenu.Add(TI_SalesOrder);
            _ListMenu.Add(TI_DeliveryOrder);

            _ListMenu.Add(TI_Trip);
            _ListMenu.Add(TI_Shipment);
            _ListMenu.Add(TI_CheckIn);
            _ListMenu.Add(TI_Queue);

            _ListMenu.Add(TI_GateGeneralReport);

            _ListMenu.Add(TI_WeWeighInReport);
            _ListMenu.Add(TI_WeGeneralReport);

            _ListMenu.Add(TI_WaGeneralReport);

            _ListMenu.Add(TI_TransportUnit);
            _ListMenu.Add(TI_Customer);

            _ListMenu.Add(TI_Item);
            _ListMenu.Add(TI_ItemCategory);

            _ListMenu.Add(TI_Warehouse);
            _ListMenu.Add(TI_Location);

            _ListMenu.Add(TI_WeighBridge);

            return _ListMenu;
        }
    }

}


