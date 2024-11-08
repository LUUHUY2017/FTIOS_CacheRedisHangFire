using AMMS.Shared.Commons;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.Infratructures.Repositories;

public class ScheduleSendEmailDetailRepository : IScheduleSendEmailDetailRepository
{
    private readonly IViettelDbContext _db;
    public ScheduleSendEmailDetailRepository(IViettelDbContext biDbContext)
    {
        _db = biDbContext;
    }

    public Task<bool> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<ScheduleSendMailDetail>>> Get(string sheduleId)
    {
        try
        {
            var _data = await (from _do in _db.ScheduleSendMailDetail
                               where _do.ScheduleId == sheduleId
                               select _do).ToListAsync();

            return new Result<List<ScheduleSendMailDetail>>(_data, "Thành công!", true);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<Result<ScheduleSendMailDetail>> UpdateAsync(ScheduleSendMailDetail data)
    {
        string message = "";
        try
        {
            var _orderDetail = _db.ScheduleSendMailDetail.FirstOrDefault(o => o.Id == data.Id);
            if (_orderDetail != null)
            {
                data.CopyPropertiesTo(_orderDetail);
                _db.ScheduleSendMailDetail.Update(_orderDetail);
                message = "Cập nhật thành công";
            }
            else
            {
                _orderDetail = new ScheduleSendMailDetail();
                data.CopyPropertiesTo(_orderDetail);
                _db.ScheduleSendMailDetail.Update(_orderDetail);
                message = "Thêm mới thành công";
            }

            var retVal = await _db.SaveChangesAsync();
            return new Result<ScheduleSendMailDetail>(_orderDetail, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ScheduleSendMailDetail>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<List<ScheduleSendMailDetail>>> UpdateAsync(List<ScheduleSendMailDetail> datas)
    {
        try
        {
            if (!datas.Any())
                return new Result<List<ScheduleSendMailDetail>>(datas, "Không có dữ liệu", false);
            foreach (var data in datas)
            {
                var _orderDetail = _db.ScheduleSendMailDetail.FirstOrDefault(o => o.Id == data.Id);
                if (_orderDetail != null)
                {
                    data.CopyPropertiesTo(_orderDetail);
                    _db.ScheduleSendMailDetail.Update(_orderDetail);
                }
                else
                {
                    _orderDetail = new ScheduleSendMailDetail();
                    data.CopyPropertiesTo(_orderDetail);
                    _db.ScheduleSendMailDetail.Update(_orderDetail);
                }
            }
            var retVal = await _db.SaveChangesAsync();

            return new Result<List<ScheduleSendMailDetail>>(datas, "Cập nhật thành công", false);
        }
        catch (Exception ex)
        {
            return new Result<List<ScheduleSendMailDetail>>(datas, "Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<int>> DeleteAsync(DeleteRequest request)
    {
        try
        {

            var result = _db.ScheduleSendMailDetail.FirstOrDefault(o => o.Id == request.Id);
            if (result == null)
                return new Result<int>("Không tìm thấy dữ liệu", false);

            _db.ScheduleSendMailDetail.Remove(result);
            var retVal = await _db.SaveChangesAsync();

            return new Result<int>(retVal, "Xóa thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<int>(0, "Lỗi: " + ex.ToString(), false);
        }
    }
}
