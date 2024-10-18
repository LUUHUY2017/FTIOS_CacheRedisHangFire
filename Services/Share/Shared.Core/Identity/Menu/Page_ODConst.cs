 using Shared.Core.Identity.Object;

namespace Shared.Core.Identity.Menu;

public partial class PagesConst
{
    #region OD
    public static readonly PageObject OD_Home = new PageObject
    {
        Id = "c8fe902e-cfb7-4cee-8d8f-11759aa171b4",
        Name = "Dashboard",
        Url = "V1/OD_Home",
        Module = Module.Delivery,
        RolePermission = RoleConst.DashboardPage,
        CategoryMenu = Category.Dashboard.Id,
    };

    public static readonly PageObject OD_Contract = new PageObject
    {
        Id = "bd4111c4-caed-4a11-bb6b-73113eee4e47",
        Url = "OD/v1/Contract",
        Name = "Hợp đồng",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.QuanLyVanTai.Id,
    };

    public static readonly PageObject OD_SalesOrder = new PageObject
    {
        Id = "afdf3e35-5756-4476-99af-54210c53c3aa",
        Name = "Đơn hàng",
        Url = "OD/v1/SalesOrder",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.QuanLyVanTai.Id,
    };

    public static readonly PageObject OD_DeliveryOrder = new PageObject
    {
        Id = "afcb32600-45b3-464b-bcf0-1c1046ad2e52",
        Name = "Kế hoạch vận chuyển",
        Url = "OD/v1/DeliveryOrder",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.QuanLyVanTai.Id,
    };
    public static readonly PageObject OD_Trip = new PageObject
    {
        Id = "3593ae26-4b3b-4d09-8437-e6efdbfe7563",
        Url = "OD/v1/Trip",
        Name = "Chuyến xe",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.QuanLyVanTai.Id,
    };
    public static readonly PageObject OD_Shipment = new PageObject
    {
        Id = "96137e5e-c5e0-420b-ac35-a9448f2c69e0",
        Url = "OD/v1/Shipment",
        Name = "Vận đơn",
        RolePermission = RoleConst.MasterDataPage,
        Module = Module.Delivery,
        CategoryMenu = Category.QuanLyVanTai.Id,
    };

    public static readonly PageObject OD_CheckIn = new PageObject
    {
        Id = "d5c2b3bc-54e3-4661-a576-006c9ae58a9c",
        Url = "OD/V1/Checkin",
        Name = "Đăng tài",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.XepLuot.Id,
    };
    public static readonly PageObject OD_Queue = new PageObject
    {
        Id = "5cd1b334-0572-46b7-9b0f-0f6a0d4bb5db",
        Name = "Xếp tài",
        Url = "OD/V1/Queue",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.XepLuot.Id,
    };
    public static readonly PageObject OD_GateGeneralReport = new PageObject
    {
        Id = "4841a36e-934f-4461-883e-a1f6cb19a49b",
        Name = "Báo cáo",
        Url = "OD/v1/GateIO/GeneralReport",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.XeRaVaoCong.Id,
    };

    public static readonly PageObject OD_WeWeighInReport = new PageObject
    {
        Id = "b1ebbffd-aa2b-4d6e-a9fb-81991b7b500c",
        Name = "Báo cáo cân vào",
        Url = "OD/v1/WeighVoucher/WeighInReport",
        RolePermission = RoleConst.MasterDataPage,
        Module = Module.Delivery,
        CategoryMenu = Category.CauCan.Id,
    };
    public static readonly PageObject OD_WeGeneralReport = new PageObject
    {
        Id = "97b3d98d-8159-417f-acac-b4f16f89db6b",
        Url = "OD/v1/WeighVoucher/GeneralReport",
        Name = "Báo cáo tổng hợp",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.CauCan.Id,
    };

    public static readonly PageObject OD_WaGeneralReport = new PageObject
    {
        Id = "f5032f54-d0e7-4881-820c-1d84d7d311cc",
        Url = "OD/v1/Warehouse/GeneralReport",
        Name = "Báo cáo",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.XeRaVaoKho.Id,
    };

    public static readonly PageObject OD_TransportUnit = new PageObject
    {
        Id = "c9fff7c6-04f3-47a1-a8c2-a82e183ce269",
        Name = "Nhà vận tải",
        Url = "OD/v1/TransportUnit",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };

    public static readonly PageObject OD_Customer = new PageObject
    {
        Id = "1648e7d9-757d-49d2-bac5-4cce23a371de",
        Name = "Khách hàng",
        Url = "OD/v1/Customer",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };
    public static readonly PageObject OD_Item = new PageObject
    {
        Id = "11586864f-800e-42a7-bf2d-50c6ea833dea",
        Url = "OD/v1/Item",
        Name = "Hàng hóa",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };
    public static readonly PageObject OD_ItemCategory = new PageObject
    {
        Id = "5784712d-80f7-4a20-bdad-af439e8bbf9f",
        Url = "OD/v1/ItemCategory",
        Name = "Danh mục hàng hóa",
        RolePermission = RoleConst.MasterDataPage,
        Module = Module.Delivery,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };

    public static readonly PageObject OD_Warehouse = new PageObject
    {
        Id = "66023fb5-2ca4-4ef9-9691-9a9f1780cdec",
        Url = "OD/v1/Warehouse",
        Name = "Kho",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };
    public static readonly PageObject OD_Location = new PageObject
    {
        Id = "64abc1eb-98a3-47f8-af8f-eeab3f7a1299",
        Name = "Địa điểm",
        Url = "OD/v1/Location",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };
    public static readonly PageObject OD_WeighBridge = new PageObject
    {
        Id = "e50cb7e8-8771-4e6c-af6c-aa090e39cece",
        Name = "Cầu cân",
        Url = "OD/v1/WeighBridge",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.ThietLap.Id,
    };

    public static readonly PageObject OD_Process = new PageObject
    {
        Id = "d83322de-4396-43d3-8e22-a59f2cbb4283",
        Name = "Quy trình phê duyệt",
        Url = "OD/v1/Process",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.ThietLap.Id,
    };

    #endregion

    public static List<MenuShowModel> Menu_OD_Left = new List<MenuShowModel>();
    public static List<PageObject> _Menu_OD_Left
    {
        get
        {
            var _ListMenu = new List<PageObject>();

            _ListMenu.Add(OD_Home);
            _ListMenu.Add(OD_Contract);
            _ListMenu.Add(OD_SalesOrder);
            _ListMenu.Add(OD_DeliveryOrder);

            _ListMenu.Add(OD_Trip);
            _ListMenu.Add(OD_Shipment);
            _ListMenu.Add(OD_CheckIn);
            _ListMenu.Add(OD_Queue);

            _ListMenu.Add(OD_GateGeneralReport);

            _ListMenu.Add(OD_WeWeighInReport);
            _ListMenu.Add(OD_WeGeneralReport);

            _ListMenu.Add(OD_WaGeneralReport);

            _ListMenu.Add(OD_TransportUnit);
            _ListMenu.Add(OD_Customer);

            _ListMenu.Add(OD_Item);
            _ListMenu.Add(OD_ItemCategory);

            _ListMenu.Add(OD_Warehouse);
            _ListMenu.Add(OD_Location);

            _ListMenu.Add(OD_WeighBridge);
            _ListMenu.Add(OD_Process);

            return _ListMenu;
        }
    }

}


