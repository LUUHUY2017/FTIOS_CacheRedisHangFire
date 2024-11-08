
using AMMS.Shared.Commons;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.ScheduleSendEmails;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.SendEmails;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.Infratructures.Repositories.SendEmails;

public class SendEmailRepository : ISendEmailRepository
{
    private readonly IViettelDbContext _db;
    public SendEmailRepository(IViettelDbContext biDbContext)
    {
        _db = biDbContext;
    }

    public async Task<Result<List<Cores.Entities.A2.SendEmails>>> GetAlls(ScheduleSendEmailLogModel request)
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

            return new Result<List<Cores.Entities.A2.SendEmails>>(_data, "Thành công!", true);
        }
        catch (Exception ex)
        {
            return new Result<List<Cores.Entities.A2.SendEmails>>("Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<Cores.Entities.A2.SendEmails>> UpdateAsync(Cores.Entities.A2.SendEmails data)
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
                _order = new Cores.Entities.A2.SendEmails();
                data.CopyPropertiesTo(_order);
                _db.SendEmail.Add(_order);
                message = "Thêm mới thành công";
            }
            var retVal = await _db.SaveChangesAsync();
            return new Result<Cores.Entities.A2.SendEmails>(data, message, true);
        }
        catch (Exception ex)
        {
            return new Result<Cores.Entities.A2.SendEmails>(data, "Lỗi: " + ex.ToString(), false);
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
