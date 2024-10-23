using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Devices;
using Server.Core.Interfaces.A2.Lanes.Requests;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.Devices;

public class LaneRepository : ILaneRepository
{
    private readonly IMasterDataDbContext _db;
    public LaneRepository(IMasterDataDbContext biDbContext)
    {
        _db = biDbContext;
    }

    public async Task<Result<A2_Lane>> ActiveAsync(ActiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.A2_Lane.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = true;
                _db.A2_Lane.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<A2_Lane>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Lane>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<A2_Lane>> InActiveAsync(InactiveRequest data)
    {
        string message = "";
        try
        {
            var _order = _db.A2_Lane.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                _order.Actived = false;
                _db.A2_Lane.Update(_order);
                message = "Cập nhật thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<A2_Lane>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Lane>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<List<A2_Lane>> Gets(LaneFilterRequest req)
    {
        try
        {
            bool activedVal = req.Actived == "1";

            var _data = await (from o in _db.A2_Lane
                               where o.Actived == activedVal
                                    && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "LaneName" ? o.LaneName.Contains(req.Key) : true)
                                    && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "LaneName" ? o.LaneName.Contains(req.Key) : true)
                                    && (!string.IsNullOrWhiteSpace(req.Key) && req.ColumnTable == "Description" ? o.Description.Contains(req.Key) : true)
                               select o).ToListAsync();
            return _data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<List<A2_Lane>> GetAll()
    {
        try
        {
            var _data = await (from o in _db.A2_Lane
                               where o.Actived == true
                               select o).ToListAsync();
            return _data;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    public async Task<Result<A2_Lane>> GetById(string id)
    {
        string message = "";
        try
        {
            var _order = _db.A2_Lane.FirstOrDefault(o => o.Id == id);
            return new Result<A2_Lane>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Lane>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<A2_Lane>> UpdateAsync(A2_Lane data)
    {
        string message = "";
        try
        {
            var _order = await _db.A2_Lane.SingleOrDefaultAsync(o => o.Id == data.Id); // Use async version of query
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.A2_Lane.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new A2_Lane();
                data.CopyPropertiesTo(_order);
                await _db.A2_Lane.AddAsync(_order);
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

            return new Result<A2_Lane>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Lane>(data, "Lỗi: " + ex.ToString(), false);
        }
    }


}
