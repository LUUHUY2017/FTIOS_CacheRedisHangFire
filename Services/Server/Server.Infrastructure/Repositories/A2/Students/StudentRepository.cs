using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Students;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.Students;

public class StudentRepository : RepositoryBaseMasterData<A2_Student>, IStudentRepository
{
    public StudentRepository(MasterDataDbContext dbContext) : base(dbContext)
    {
    }

    public string UserId { get; set; }
    public async Task<Result<A2_Student>> SaveAsync(A2_Student data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.A2_Student.FirstOrDefaultAsync(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _order.ImageSrc = null;
                _dbContext.A2_Student.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new A2_Student();
                data.CopyPropertiesTo(_order);

                await _dbContext.A2_Student.AddAsync(_order);
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

            return new Result<A2_Student>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Student>(data, "Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<A2_Student>> SaveDataAsync(A2_Student data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.A2_Student.FirstOrDefaultAsync(o => o.StudentCode == data.StudentCode);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _order.ImageSrc = null;
                _dbContext.A2_Student.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new A2_Student();
                data.CopyPropertiesTo(_order);

                await _dbContext.A2_Student.AddAsync(_order);
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

            return new Result<A2_Student>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Student>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
}
