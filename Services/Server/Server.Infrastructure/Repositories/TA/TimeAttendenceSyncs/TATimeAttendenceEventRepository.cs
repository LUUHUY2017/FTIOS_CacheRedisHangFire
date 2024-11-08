using AMMS.Shared.Commons;
using Server.Core.Entities.TA;
using Server.Core.Interfaces.TA.TimeAttendenceSyncs;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.TA.TimeAttendenceSyncs;

public class TATimeAttendenceSyncRepository : ITATimeAttendenceSyncRepository
{
    private readonly IMasterDataDbContext _db;

    public string UserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public TATimeAttendenceSyncRepository(IMasterDataDbContext biDbContext)
    {
        _db = biDbContext;
    }

    public async Task<Result<TimeAttendenceSync>> CreateAsync(TimeAttendenceSync data)
    {
        string message = "";
        try
        {
            var _order = new TimeAttendenceSync();
            data.CopyPropertiesTo(_order);
            _db.TimeAttendenceSync.Add(_order);

            message = "Thêm mới thành công";
            var retVal = await _db.SaveChangesAsync();
            return new Result<TimeAttendenceSync>(data, message, true);
        }
        catch (Exception ex)
        {
            return new Result<TimeAttendenceSync>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<TimeAttendenceSync>> UpdateAsync(TimeAttendenceSync data)
    {
        string message = "";
        try
        {
            var _order = _db.TimeAttendenceSync.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.TimeAttendenceSync.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new TimeAttendenceSync();
                data.CopyPropertiesTo(_order);
                _db.TimeAttendenceSync.Add(_order);
                message = "Thêm mới thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<TimeAttendenceSync>(data, message, true);
        }
        catch (Exception ex)
        {
            return new Result<TimeAttendenceSync>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<TimeAttendenceSync>> UpdateStatusAsync(TimeAttendenceSync data)
    {
        string message = "";
        try
        {
            var _order = _db.TimeAttendenceSync.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                message = "Cập nhật thành công";

                _order.SyncStatus = data.SyncStatus;
                _order.Message = data.Message;
                _order.ParamResponses = data.ParamResponses;
                _db.TimeAttendenceSync.Update(_order);
            }
            else
            {
                _order = new TimeAttendenceSync();
                data.CopyPropertiesTo(_order);
                _db.TimeAttendenceSync.Add(_order);

                message = "Thêm mới thành công";
            }
            var retVal = await _db.SaveChangesAsync();

            return new Result<TimeAttendenceSync>(data, message, true);
        }
        catch (Exception ex)
        {
            return new Result<TimeAttendenceSync>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<int>> DeleteAsync(string id)
    {
        try
        {
            var result = _db.TimeAttendenceSync.FirstOrDefault(o => o.Id == id);
            if (result == null)
                return new Result<int>("Không tìm thấy dữ liệu", false);

            _db.TimeAttendenceSync.Remove(result);
            var retVal = await _db.SaveChangesAsync();

            return new Result<int>(retVal, "Xóa thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<int>(0, "Lỗi: " + ex.ToString(), false);
        }
    }




}