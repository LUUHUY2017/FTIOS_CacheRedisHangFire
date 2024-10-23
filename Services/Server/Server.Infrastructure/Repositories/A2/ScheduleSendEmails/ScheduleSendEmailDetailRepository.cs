using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleSendEmails;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.ScheduleSendEmails;

public class ScheduleSendEmailDetailRepository : IScheduleSendEmailDetailRepository
{
    private readonly IMasterDataDbContext _db;
    public ScheduleSendEmailDetailRepository(IMasterDataDbContext biDbContext)
    {
        _db = biDbContext;
    }

    public Task<bool> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<List<A2_ScheduleSendMailDetail>>> Get(string sheduleId)
    {
        try
        {
            var _data = await (from _do in _db.A2_ScheduleSendMailDetail
                               where _do.ScheduleId == sheduleId
                               select _do).ToListAsync();

            return new Result<List<A2_ScheduleSendMailDetail>>(_data, "Thành công!", true);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public async Task<Result<A2_ScheduleSendMailDetail>> UpdateAsync(A2_ScheduleSendMailDetail data)
    {
        string message = "";
        try
        {
            var _orderDetail = _db.A2_ScheduleSendMailDetail.FirstOrDefault(o => o.Id == data.Id);
            if (_orderDetail != null)
            {
                data.CopyPropertiesTo(_orderDetail);
                _db.A2_ScheduleSendMailDetail.Update(_orderDetail);
                message = "Cập nhật thành công";
            }
            else
            {
                _orderDetail = new A2_ScheduleSendMailDetail();
                data.CopyPropertiesTo(_orderDetail);
                _db.A2_ScheduleSendMailDetail.Update(_orderDetail);
                message = "Thêm mới thành công";
            }

            var retVal = await _db.SaveChangesAsync();
            return new Result<A2_ScheduleSendMailDetail>(_orderDetail, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_ScheduleSendMailDetail>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<List<A2_ScheduleSendMailDetail>>> UpdateAsync(List<A2_ScheduleSendMailDetail> datas)
    {
        try
        {
            if (!datas.Any())
                return new Result<List<A2_ScheduleSendMailDetail>>(datas, "Không có dữ liệu", false);
            foreach (var data in datas)
            {
                var _orderDetail = _db.A2_ScheduleSendMailDetail.FirstOrDefault(o => o.Id == data.Id);
                if (_orderDetail != null)
                {
                    data.CopyPropertiesTo(_orderDetail);
                    _db.A2_ScheduleSendMailDetail.Update(_orderDetail);
                }
                else
                {
                    _orderDetail = new A2_ScheduleSendMailDetail();
                    data.CopyPropertiesTo(_orderDetail);
                    _db.A2_ScheduleSendMailDetail.Update(_orderDetail);
                }
            }
            var retVal = await _db.SaveChangesAsync();

            return new Result<List<A2_ScheduleSendMailDetail>>(datas, "Cập nhật thành công", false);
        }
        catch (Exception ex)
        {
            return new Result<List<A2_ScheduleSendMailDetail>>(datas, "Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<int>> DeleteAsync(DeleteRequest request)
    {
        try
        {

            var result = _db.A2_ScheduleSendMailDetail.FirstOrDefault(o => o.Id == request.Id);
            if (result == null)
                return new Result<int>("Không tìm thấy dữ liệu", false);

            _db.A2_ScheduleSendMailDetail.Remove(result);
            var retVal = await _db.SaveChangesAsync();

            return new Result<int>(retVal, "Xóa thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<int>(0, "Lỗi: " + ex.ToString(), false);
        }
    }
}
