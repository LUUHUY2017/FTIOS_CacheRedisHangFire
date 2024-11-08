using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A0;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.ScheduleSendEmails.Requests;
using Server.Core.Interfaces.A2.SendEmails;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.SendEmails;

public class SendEmailRepository : ISendEmailRepository
{
    private readonly IMasterDataDbContext _db;
    public SendEmailRepository(IMasterDataDbContext biDbContext)
    {
        _db = biDbContext;
    }

    public async Task<Result<List<Core.Entities.A2.SendEmails>>> GetAlls(ScheduleSendEmailLogModel request)
    {
        try
        {
            var sent = request.Sent;
            bool? statusSent = null;
            if (sent == "1")
                statusSent = true;
            else if (sent == "0")
                statusSent = false;

            var _data = await (from _do in _db.SendEmail
                               where
                                (request.StartDate != null ? _do.CreatedDate.Date >= request.StartDate.Value.Date : true)
                                && (request.EndDate != null ? _do.CreatedDate.Date <= request.EndDate.Value.Date : true)
                                && (request.OrganizationId != "0" ? _do.OrganizationId == request.OrganizationId : true)
                                && (string.IsNullOrWhiteSpace(request.Sent) ? true : _do.Sent == statusSent)
                               orderby _do.TimeSent descending
                               orderby _do.CreatedDate descending
                               select _do).ToListAsync();

            return new Result<List<Core.Entities.A2.SendEmails>>(_data, "Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<Core.Entities.A2.SendEmails>>("Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<Core.Entities.A2.SendEmails>> UpdateAsync(Core.Entities.A2.SendEmails data)
    {
        string message = "";
        try
        {
            var _order = _db.SendEmail.FirstOrDefault(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _db.SendEmail.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new Core.Entities.A2.SendEmails();
                data.CopyPropertiesTo(_order);
                _db.SendEmail.Add(_order);
                message = "Thêm mới thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<Core.Entities.A2.SendEmails>(data, message, true);
        }
        catch (Exception ex)
        {
            return new Result<Core.Entities.A2.SendEmails>(data, "Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<int>> DeleteAsync(string id)
    {
        try
        {
            var result = _db.SendEmail.FirstOrDefault(o => o.Id == id);
            if (result == null)
                return new Result<int>("Không tìm thấy dữ liệu", false);

            _db.SendEmail.Remove(result);
            var retVal = await _db.SaveChangesAsync();

            return new Result<int>(retVal, "Xóa thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<int>(0, "Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<EmailConfiguration>> GetEmailConfiguration(string orgId)
    {
        string message = "";
        try
        {
            var _order = _db.EmailConfiguration.FirstOrDefault(o => o.OrganizationId == orgId);
            return new Result<EmailConfiguration>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<EmailConfiguration>("Lỗi: " + ex.ToString(), false);
        }
    }

}
