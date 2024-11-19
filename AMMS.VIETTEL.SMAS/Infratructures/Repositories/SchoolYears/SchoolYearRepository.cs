using AMMS.Shared.Commons;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.SchoolYears;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.Infratructures.Repositories.SchoolYears;

public class SchoolYearRepository : RepositoryBase<SchoolYear>, ISchoolYearRepository
{
    public SchoolYearRepository(ViettelDbContext dbContext) : base(dbContext)
    {
    }

    public string UserId { get; set; }
    public async Task<Result<SchoolYear>> SaveAsync(SchoolYear data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.SchoolYear.FirstOrDefaultAsync(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _dbContext.SchoolYear.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new SchoolYear();
                data.CopyPropertiesTo(_order);

                await _dbContext.SchoolYear.AddAsync(_order);
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

            return new Result<SchoolYear>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<SchoolYear>(data, "Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<SchoolYear>> SaveDataAsync(SchoolYear data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.SchoolYear.FirstOrDefaultAsync(o => o.Name == data.Name);
            if (_order != null)
            {
                _order.ReferenceId = data.Id;
                _order.LastModifiedDate = DateTime.Now;
                _order.Name = data.Name;
                _order.Depcription = data.Depcription;
                _order.Start = data.Start;
                _order.End = data.End;
                _order.OrganizationId = data.OrganizationId;

                _dbContext.SchoolYear.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new SchoolYear();
                data.CopyPropertiesTo(_order);
                _order.Id = !string.IsNullOrWhiteSpace(data.Id) ? Guid.NewGuid().ToString() : data.Id;
                await _dbContext.SchoolYear.AddAsync(_order);
                message = "Thêm mới thành công";
            }

            try
            {
                var retVal = await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                message = $"Error occurred: {ex.Message}";
            }

            return new Result<SchoolYear>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<SchoolYear>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
}
