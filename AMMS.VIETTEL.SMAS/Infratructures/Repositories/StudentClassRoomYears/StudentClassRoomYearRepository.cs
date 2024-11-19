using AMMS.Shared.Commons;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.StudentClassRoomYears;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.Infratructures.Repositories.StudentClassRoomYears;

public class StudentClassRoomYearRepository : RepositoryBase<StudentClassRoomYear>, IStudentClassRoomYearRepository
{
    public StudentClassRoomYearRepository(ViettelDbContext dbContext) : base(dbContext)
    {
    }

    public string UserId { get; set; }
    public async Task<Result<StudentClassRoomYear>> SaveAsync(StudentClassRoomYear data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.StudentClassRoomYear.FirstOrDefaultAsync(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _dbContext.StudentClassRoomYear.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new StudentClassRoomYear();
                data.CopyPropertiesTo(_order);
                await _dbContext.StudentClassRoomYear.AddAsync(_order);
                message = "Thêm mới thành công";
            }
            var retVal = await _dbContext.SaveChangesAsync();

            return new Result<StudentClassRoomYear>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<StudentClassRoomYear>(data, "Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<StudentClassRoomYear>> SaveDataAsync(StudentClassRoomYear data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.StudentClassRoomYear.FirstOrDefaultAsync(o => o.SchoolId == data.SchoolId && o.ClassRoomId == data.ClassRoomId && o.SchoolYearId == data.SchoolYearId);
            if (_order != null)
            {
                _order.ReferenceId = data.Id;
                _order.LastModifiedDate = DateTime.Now;

                _order.Name = data.Name;

                _order.SchoolId = data.SchoolId;
                _order.ClassRoomId = data.ClassRoomId;
                _order.SchoolYearId = data.SchoolYearId;
                _order.OrganizationId = data.OrganizationId;

                _dbContext.StudentClassRoomYear.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new StudentClassRoomYear();
                data.CopyPropertiesTo(_order);
                _order.Id = !string.IsNullOrWhiteSpace(data.Id) ? Guid.NewGuid().ToString() : data.Id;
                await _dbContext.StudentClassRoomYear.AddAsync(_order);
                message = "Thêm mới thành công";
            }
            var retVal = await _dbContext.SaveChangesAsync();

            return new Result<StudentClassRoomYear>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<StudentClassRoomYear>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
}
