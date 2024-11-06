using AMMS.Shared.Commons;
using Microsoft.EntityFrameworkCore;
using Server.Core.Entities.A0;
using Server.Core.Entities.A2;
using Server.Core.Interfaces.A2.Persons;
using Server.Infrastructure.Datas.MasterData;
using Shared.Core.Commons;

namespace Server.Infrastructure.Repositories.A2.Persons;

public class PersonRepository : RepositoryBaseMasterData<A2_Person>, IPersonRepository
{

    public string UserId { get; set; }
    public PersonRepository(MasterDataDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result<A2_Person>> SaveAsync(A2_Person cus)
    {
        string message = "";
        try
        {
            var org = await _dbContext.A2_Person.FirstOrDefaultAsync(o => o.Id == cus.Id);
            if (org != null)
            {
                cus.CopyPropertiesTo(org);
                await UpdateAsync(org);
                message = "Cập nhật thành công";
            }
            else
            {
                org = new A2_Person();
                cus.CopyPropertiesTo(org);
                await AddAsync(org);
                message = "Thêm mới thành công";
            }
            return new Result<A2_Person>(org, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_Person>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<A2_PersonFace>> SaveImageAsync(string personId, string base64String)
    {
        string message = "";
        try
        {
            var org = await _dbContext.A2_PersonFace.FirstOrDefaultAsync(o => o.PersonId == personId);
            if (org != null)
            {
                org.FaceData = base64String;
                _dbContext.A2_PersonFace.Update(org);
                message = "Cập nhật thành công";
            }
            else
            {
                org = new A2_PersonFace();
                org.PersonId = personId;
                org.FaceIndex = 1;
                org.FaceData = base64String;
                _dbContext.A2_PersonFace.Add(org);
                message = "Thêm mới thành công";
            }
            await _dbContext.SaveChangesAsync();

            return new Result<A2_PersonFace>(org, message, true);
        }
        catch (Exception ex)
        {
            return new Result<A2_PersonFace>("Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<A2_PersonFace>> GetFacePersonById(string id)
    {
        try
        {
            var _order = await _dbContext.A2_PersonFace.FirstOrDefaultAsync(o => o.PersonId == id);
            if (_order == null)
                return new Result<A2_PersonFace>("Không có khuôn mặt", true);

            return new Result<A2_PersonFace>(_order, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<A2_PersonFace>("Lỗi: " + ex.ToString(), false);
        }
    }
}
