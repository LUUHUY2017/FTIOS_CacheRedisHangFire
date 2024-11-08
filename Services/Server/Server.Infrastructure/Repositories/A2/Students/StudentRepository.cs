using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Students;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.Students;

public class StudentRepository : RepositoryBaseMasterData<Student>, IStudentRepository
{
    public StudentRepository(MasterDataDbContext dbContext) : base(dbContext)
    {
    }

    public string UserId { get; set; }
    public async Task<Result<Student>> SaveAsync(Student data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.Student.FirstOrDefaultAsync(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _order.ImageSrc = null;
                _dbContext.Student.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new Student();
                data.CopyPropertiesTo(_order);

                await _dbContext.Student.AddAsync(_order);
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

            return new Result<Student>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<Student>(data, "Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<Student>> SaveDataAsync(Student data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.Student.FirstOrDefaultAsync(o => o.StudentCode == data.StudentCode);
            if (_order != null)
            {
                _order.ReferenceId = data.Id;
                _order.ClassId = data.ClassId;
                _order.ClassName = data.ClassName;
                _order.OrganizationId = data.OrganizationId;
                _order.StudentCode = data.StudentCode;
                _order.Name = data.Name;
                _order.FullName = data.FullName;
                _order.DateOfBirth = data.DateOfBirth;
                _order.GenderCode = data.GenderCode;
                _order.GradeCode = data.GradeCode;


                _dbContext.Student.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new Student();
                data.CopyPropertiesTo(_order);

                await _dbContext.Student.AddAsync(_order);
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

            return new Result<Student>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<Student>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
}
