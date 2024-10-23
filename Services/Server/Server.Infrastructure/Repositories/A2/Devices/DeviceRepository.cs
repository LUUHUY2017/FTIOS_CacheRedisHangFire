using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Devices;
using Server.Core.Interfaces.A2.Devices.RequeResponsessts;
using Server.Core.Interfaces.A2.Devices.Requests;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.Devices;

public class DeviceRepository : IDeviceRepository
{
    private readonly IMasterDataDbContext _db;
    public DeviceRepository(IMasterDataDbContext biDbContext)
    {
        _db = biDbContext;
    }

    public async Task<Result<A2_Device>> ActiveAsync(ActiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.A2_Device.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = true;
                _db.A2_Device.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<A2_Device>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Device>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<A2_Device>> InActiveAsync(InactiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.A2_Device.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = false;
                _db.A2_Device.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<A2_Device>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Device>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<List<DeviceResponse>> Gets(DeviceFilterRequest req)
    {
        try
        {
            bool activedVal = req.Actived == "1";

            var _data = await (from o in _db.A2_Device
                               join _la in _db.A2_Lane on o.LaneId equals _la.Id into K
                               from la in K.DefaultIfEmpty()
                               where o.Actived == activedVal
                                && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "DeviceName" ? o.DeviceName.Contains(req.Key) : true)
                                && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "DeviceCode" ? o.DeviceCode.Contains(req.Key) : true)
                                && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "DeviceDescription" ? o.DeviceDescription.Contains(req.Key) : true)
                                && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "SerialNumber" ? o.SerialNumber.Contains(req.Key) : true)
                                && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "IPAddress" ? o.IPAddress.Contains(req.Key) : true)
                               select new DeviceResponse()
                               {
                                   Id = o.Id,
                                   Actived = o.Actived,
                                   CreatedBy = o.CreatedBy,
                                   CreatedDate = o.CreatedDate,

                                   DeviceIn = o.DeviceIn,
                                   DeviceName = o.DeviceName,
                                   SerialNumber = o.SerialNumber,
                                   DeviceID = o.DeviceID,
                                   Password = o.Password,

                                   ActiveKey = o.ActiveKey,
                                   DeviceCode = o.DeviceCode,
                                   DeviceDescription = o.DeviceDescription,
                                   DeviceInfo = o.DeviceInfo,
                                   DeviceInput = o.DeviceInput,
                                   IPAddress = o.IPAddress,
                                   MacAddress = o.MacAddress,
                                   HTTPPort = o.HTTPPort,
                                   PortConnect = o.PortConnect,
                                   ConnectionStatus = o.ConnectionStatus,
                                   BrandName = o.BrandName,
                                   DeviceType = o.DeviceType,
                                   OrganizationId = o.OrganizationId,
                                   GateId = o.GateId,

                                   LaneId = o.LaneId,
                                   LaneName = la != null ? la.LaneName : "",
                               }).ToListAsync();
            return _data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<List<A2_Device>> GetAll()
    {
        try
        {
            var _data = await (from o in _db.A2_Device
                               where o.Actived == true
                               select o).ToListAsync();
            return _data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<Result<A2_Device>> GetById(string id)
    {
        string message = "";
        try
        {
            var _order = _db.A2_Device.FirstOrDefault(o => o.Id == id);
            return new Result<A2_Device>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Device>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<A2_Device>> UpdateAsync(A2_Device data)
    {
        string message = "";
        try
        {
            var _order = await _db.A2_Device.SingleOrDefaultAsync(o => o.Id == data.Id); // Use async version of query
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.A2_Device.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new A2_Device();
                data.CopyPropertiesTo(_order);
                await _db.A2_Device.AddAsync(_order);
                message = "Thêm mới thành công";
            }

            try
            {
                var retVal = await _db.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                // Log and handle the exception, if necessary
                message = $"Error occurred: {ex.Message}";
            }

            return new Result<A2_Device>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Device>(data, "Lỗi: " + ex.ToString(), false);
        }
    }


}
