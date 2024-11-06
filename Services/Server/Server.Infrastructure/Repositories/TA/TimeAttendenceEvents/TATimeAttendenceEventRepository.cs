using AMMS.Shared.Commons;
using Server.Core.Entities.TA;
using Server.Core.Interfaces.TA.TimeAttendenceEvents;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.TA.TimeAttendenceEvents;

public class TATimeAttendenceEventRepository : ITATimeAttendenceEventRepository
{
    private readonly IMasterDataDbContext _db;
    public TATimeAttendenceEventRepository(IMasterDataDbContext biDbContext)
    {
        _db = biDbContext;
    }

    public async Task<Result<TA_TimeAttendenceEvent>> CreateAsync(TA_TimeAttendenceEvent data)
    {
        string message = "";
        try
        {
            var _order = new TA_TimeAttendenceEvent();
            data.CopyPropertiesTo(_order);
            _db.TA_TimeAttendenceEvent.Add(_order);

            message = "Thêm mới thành công";
            var retVal = await _db.SaveChangesAsync();
            return new Result<TA_TimeAttendenceEvent>(data, message, true);
        }
        catch (Exception ex)
        {
            return new Result<TA_TimeAttendenceEvent>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<TA_TimeAttendenceEvent>> UpdateAsync(TA_TimeAttendenceEvent data)
    {
        string message = "";
        try
        {
            var _order = _db.TA_TimeAttendenceEvent.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.TA_TimeAttendenceEvent.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new TA_TimeAttendenceEvent();
                data.CopyPropertiesTo(_order);
                _db.TA_TimeAttendenceEvent.Add(_order);
                message = "Thêm mới thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<TA_TimeAttendenceEvent>(data, message, true);
        }
        catch (Exception ex)
        {
            return new Result<TA_TimeAttendenceEvent>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<int>> DeleteAsync(string id)
    {
        try
        {
            var result = _db.TA_TimeAttendenceEvent.FirstOrDefault(o => o.Id == id);
            if (result == null)
                return new Result<int>("Không tìm thấy dữ liệu", false);

            _db.TA_TimeAttendenceEvent.Remove(result);
            var retVal = await _db.SaveChangesAsync();

            return new Result<int>(retVal, "Xóa thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<int>(0, "Lỗi: " + ex.ToString(), false);
        }
    }





}