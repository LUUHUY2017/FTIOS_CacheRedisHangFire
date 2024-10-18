using Shared.Core.Identity.Object;

namespace Shared.Core.Identity.Menu;
public partial class PagesConst
{
    #region ID
    public static readonly PageObject ID_Home = new PageObject
    {
        Id = "9ddf9f7d-9d64-48c9-a9a6-8fe3aaba6c8b",
        Name = "Dashboard",
        Url = "V1/ID_Home",
        Module = Module.Delivery,
        RolePermission = RoleConst.DashboardPage,
        CategoryMenu = Category.Dashboard.Id,
    };

    public static readonly PageObject ID_Contract = new PageObject
    {
        Id = "c84e1ab9-2b7b-46ff-91b4-c68a0e0fb0df",
        Url = "ID/v1/Contract",
        Name = "Hợp đồng",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.QuanLyVanTai.Id,
    };

    public static readonly PageObject ID_SalesOrder = new PageObject
    {
        Id = "ee7b37b4-a192-46ae-a85a-798822328fd0",
        Name = "Đơn hàng",
        Url = "ID/v1/SalesOrder",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.QuanLyVanTai.Id,
    };

    public static readonly PageObject ID_DeliveryOrder = new PageObject
    {
        Id = "5d596642-e4d1-4eb6-a50d-829ad1ef0e1a",
        Name = "Kế hoạch vận chuyển",
        Url = "ID/v1/DeliveryOrder",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.QuanLyVanTai.Id,
    };
    public static readonly PageObject ID_Trip = new PageObject
    {
        Id = "cc104fa9-1d3a-4d42-9578-7f99a8516afa",
        Url = "ID/v1/Trip",
        Name = "Chuyến xe",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.QuanLyVanTai.Id,
    };
    public static readonly PageObject ID_Shipment = new PageObject
    {
        Id = "6157505d-32da-4ea4-90c4-9ccbb7f10b64",
        Url = "ID/v1/Shipment",
        Name = "Vận đơn",
        RolePermission = RoleConst.MasterDataPage,
        Module = Module.Delivery,
        CategoryMenu = Category.QuanLyVanTai.Id,
    };

    public static readonly PageObject ID_CheckIn = new PageObject
    {
        Id = "8a7e9f11-607b-4694-aa2e-ff1530f8c08f",
        Url = "ID/V1/Checkin",
        Name = "Đăng tài",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.XepLuot.Id,
    };
    public static readonly PageObject ID_Queue = new PageObject
    {
        Id = "9aecdd1f-d707-47b3-912a-7163c0e5b35f",
        Name = "Xếp tài",
        Url = "ID/V1/Queue",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.XepLuot.Id,
    };
    public static readonly PageObject ID_GateGeneralReport = new PageObject
    {
        Id = "5530ad98-f132-431c-b08d-c4afbc484bed",
        Name = "Báo cáo",
        Url = "ID/v1/GateIO/GeneralReport",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.XeRaVaoCong.Id,
    };

    public static readonly PageObject ID_WeWeighInReport = new PageObject
    {
        Id = "c2268a70-46b3-439e-8de7-7335db76481c",
        Name = "Báo cáo cân vào",
        Url = "ID/v1/WeighVoucher/WeighInReport",
        RolePermission = RoleConst.MasterDataPage,
        Module = Module.Delivery,
        CategoryMenu = Category.CauCan.Id,
    };
    public static readonly PageObject ID_WeGeneralReport = new PageObject
    {
        Id = "7e403dec-11ee-43b8-82b6-b1c09a86ccd6",
        Url = "ID/v1/WeighVoucher/GeneralReport",
        Name = "Báo cáo tổng hợp",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.CauCan.Id,
    };

    public static readonly PageObject ID_WaGeneralReport = new PageObject
    {
        Id = "ea6a5c13-6504-4678-aebe-b430a66d6438",
        Url = "ID/v1/Warehouse/GeneralReport",
        Name = "Báo cáo",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.XeRaVaoKho.Id,
    };

    public static readonly PageObject ID_TransportUnit = new PageObject
    {
        Id = "e97e7715-a7c0-48dc-aae5-d20c6ce37ef8",
        Name = "Nhà vận tải",
        Url = "ID/v1/TransportUnit",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };

    public static readonly PageObject ID_Customer = new PageObject
    {
        Id = "d99b3aae-1d23-48c4-9164-d73c55211c19",
        Name = "Khách hàng",
        Url = "ID/v1/Customer",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };
    public static readonly PageObject ID_Item = new PageObject
    {
        Id = "d07886aa-2831-47b2-8552-fdefe12faf0e",
        Url = "ID/v1/Item",
        Name = "Hàng hóa",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };
    public static readonly PageObject ID_ItemCategory = new PageObject
    {
        Id = "3ef76728-7f20-4061-a471-90d51951e891",
        Url = "ID/v1/ItemCategory",
        Name = "Danh mục hàng hóa",
        RolePermission = RoleConst.MasterDataPage,
        Module = Module.Delivery,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };

    public static readonly PageObject ID_Warehouse = new PageObject
    {
        Id = "d69b2a40-e3f4-43da-adbb-1676a880acaa",
        Url = "ID/v1/Warehouse",
        Name = "Kho",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };
    public static readonly PageObject ID_Location = new PageObject
    {
        Id = "09d99e41-064f-494b-864f-5b1cb52cad08",
        Name = "Địa điểm",
        Url = "ID/v1/Location",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.KhaiBaoDuLieu.Id,
    };
    public static readonly PageObject ID_WeighBridge = new PageObject
    {
        Id = "208458f3-288a-47bd-8624-b60310d5e0ba",
        Name = "Cầu cân",
        Url = "ID/v1/WeighBridge",
        Module = Module.Delivery,
        RolePermission = RoleConst.MasterDataPage,
        CategoryMenu = Category.ThietLap.Id,
    };
    #endregion

    public static List<MenuShowModel> Menu_ID_Left = new List<MenuShowModel>();
    public static List<PageObject> _Menu_ID_Left
    {
        get
        {
            var _ListMenu = new List<PageObject>();

            _ListMenu.Add(ID_Home);
            _ListMenu.Add(ID_Contract);
            _ListMenu.Add(ID_SalesOrder);
            _ListMenu.Add(ID_DeliveryOrder);

            _ListMenu.Add(ID_Trip);
            _ListMenu.Add(ID_Shipment);
            _ListMenu.Add(ID_CheckIn);
            _ListMenu.Add(ID_Queue);

            _ListMenu.Add(ID_GateGeneralReport);

            _ListMenu.Add(ID_WeWeighInReport);
            _ListMenu.Add(ID_WeGeneralReport);

            _ListMenu.Add(ID_WaGeneralReport);

            _ListMenu.Add(ID_TransportUnit);
            _ListMenu.Add(ID_Customer);

            _ListMenu.Add(ID_Item);
            _ListMenu.Add(ID_ItemCategory);

            _ListMenu.Add(ID_Warehouse);
            _ListMenu.Add(ID_Location);

            _ListMenu.Add(ID_WeighBridge);

            return _ListMenu;
        }
    }

}


