using AMMS.Shared.Commons;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.ClassRooms;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.Infratructures.Repositories.ClassRooms;

public class ClassRoomRepository : RepositoryBase<ClassRoom>, IClassRoomRepository
{
    public ClassRoomRepository(ViettelDbContext dbContext) : base(dbContext)
    {
    }

    public string UserId { get; set; }
    public async Task<Result<ClassRoom>> SaveAsync(ClassRoom data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.ClassRoom.FirstOrDefaultAsync(o => o.Id == data.Id);
            if (_order != null)
            {
                data.CopyPropertiesTo(_order);
                _dbContext.ClassRoom.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new ClassRoom();
                data.CopyPropertiesTo(_order);

                await _dbContext.ClassRoom.AddAsync(_order);
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

            return new Result<ClassRoom>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ClassRoom>(data, "Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<ClassRoom>> SaveDataAsync(ClassRoom data)
    {
        string message = "";
        try
        {
            var _order = await _dbContext.ClassRoom.FirstOrDefaultAsync(o => o.SchoolId == data.SchoolId && o.Name == data.Name);
            if (_order != null)
            {
                _order.ReferenceId = data.Id;
                _order.LastModifiedDate = DateTime.Now;
                _order.Name = data.Name;
                _order.OrganizationId = data.OrganizationId;
                _order.SchoolId = data.SchoolId;
                _dbContext.ClassRoom.Update(_order);
                message = "Cập nhật thành công";
            }
            else
            {
                _order = new ClassRoom();
                data.CopyPropertiesTo(_order);
                _order.Id = !string.IsNullOrWhiteSpace(data.Id) ? Guid.NewGuid().ToString() : data.Id;
                await _dbContext.ClassRoom.AddAsync(_order);
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

            return new Result<ClassRoom>(_order, message, true);
        }
        catch (Exception ex)
        {
            return new Result<ClassRoom>(data, "Lỗi: " + ex.ToString(), false);
        }
    }
}
