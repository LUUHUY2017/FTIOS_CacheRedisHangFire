using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Devices;
using Server.Core.Interfaces.A2.Devices.RequeResponsessts;
using Server.Core.Interfaces.A2.Devices.Requests;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.Devices;

public class DeviceRepository : RepositoryBaseMasterData<Device>, IDeviceRepository
{
    public DeviceRepository(MasterDataDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<List<DeviceResponse>> GetFilters(DeviceFilterRequest req)
    {
        try
        {
            bool activedVal = req.Actived == "1";

            var _data = await (from o in _dbContext.Device
                               where o.Actived == activedVal
                                && ((!string.IsNullOrEmpty(req.OrganizationId) && req.OrganizationId != "0") ? req.OrganizationId == o.OrganizationId : true)
                                && ((!string.IsNullOrEmpty(req.DeviceModel) && req.DeviceModel != "0") ? req.DeviceModel == o.DeviceModel : true)
                                && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "DeviceName" ? o.DeviceName.Contains(req.Key) : true)
                                //&& (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "DeviceCode" ? o.DeviceCode.Contains(req.Key) : true)
                                //&& (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "DeviceDescription" ? o.DeviceDescription.Contains(req.Key) : true)
                                && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "SerialNumber" ? o.SerialNumber.Contains(req.Key) : true)
                                && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "IPAddress" ? o.IPAddress.Contains(req.Key) : true)
                               select new DeviceResponse(o))
                               .ToListAsync();
            return _data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<List<DeviceResponse>> GetByOrgId(bool actived, string organizationId)
    {
        try
        {
            var _data = await (from o in _dbContext.Device
                               where (actived != null ? o.Actived == actived : true)
                                && ((!string.IsNullOrEmpty(organizationId) && organizationId != "0") ? organizationId == o.OrganizationId : true)
                               select new DeviceResponse(o))
                               .ToListAsync();
            return _data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<Result<Device>> UpdateAsyncV2(Device data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.Device.SingleOrDefaultAsync(o => o.Id == data.Id); // Use async version of query
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _dbContext.Device.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new Device();
                data.CopyPropertiesTo(_order);
                await _dbContext.Device.AddAsync(_order);
                message = "Thêm mới thành công";
            }

            try
            {
                var retVal = await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                // Log and handle the exception, if necessary
                message = $"Error occurred: {ex.Message}";
            }

            return new Result<Device>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<Device>(data, "Lỗi: " + ex.ToString(), false);
        }
    }


}
