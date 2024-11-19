using AMMS.Shared.Commons;
using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using AMMS.VIETTEL.SMAS.Cores.Interfaces.Persons;
using AMMS.VIETTEL.SMAS.Infratructures.Databases;
using Microsoft.EntityFrameworkCore;
using Shared.Core.Commons;

namespace AMMS.VIETTEL.SMAS.Infratructures.Repositories.Persons;

public class PersonRepository : RepositoryBase<Person>, IPersonRepository
{

    public string UserId { get; set; }
    public PersonRepository(ViettelDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Result<Person>> SaveAsync(Person cus)
    {
        string message = "";
        try
        {
            var org = await _dbContext.Person.FirstOrDefaultAsync(o => o.Id == cus.Id);
            if (org != null)
            {
                cus.CopyPropertiesTo(org);
                await UpdateAsync(org);
                message = "Cập nhật thành công";
            }
            else
            {
                org = new Person();
                cus.CopyPropertiesTo(org);
                await AddAsync(org);
                message = "Thêm mới thành công";
            }
            return new Result<Person>(org, message, true);
        }
        catch (Exception ex)
        {
            return new Result<Person>("Lỗi: " + ex.ToString(), false);
        }
    }
    public async Task<Result<PersonFace>> SaveImageAsync(string personId, string base64String, string folderName)
    {
        string message = "";
        try
        {
            var org = await _dbContext.PersonFace.FirstOrDefaultAsync(o => o.PersonId == personId);
            if (org != null)
            {
                //org.FaceData = base64String;
                org.FaceUrl = folderName;
                _dbContext.PersonFace.Update(org);
                message = "Cập nhật thành công";
            }
            else
            {
                org = new PersonFace();
                org.PersonId = personId;
                org.FaceIndex = 1;
                org.FaceUrl = folderName;
                //org.FaceData = base64String;
                _dbContext.PersonFace.Add(org);
                message = "Thêm mới thành công";
            }
            await _dbContext.SaveChangesAsync();

            return new Result<PersonFace>(org, message, true);
        }
        catch (Exception ex)
        {
            return new Result<PersonFace>("Lỗi: " + ex.ToString(), false);
        }
    }

    public async Task<Result<PersonFace>> GetFacePersonById(string id)
    {
        try
        {
            var _order = await _dbContext.PersonFace.FirstOrDefaultAsync(o => o.PersonId == id);
            if (_order == null)
                return new Result<PersonFace>("Không có khuôn mặt", true);

            return new Result<PersonFace>(_order, "Thành công", true);
        }
        catch (Exception ex)
        {
            return new Result<PersonFace>("Lỗi: " + ex.ToString(), false);
        }
    }
}
