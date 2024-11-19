using AMMS.VIETTEL.SMAS.Cores.Entities.A2;
using Shared.Core.Commons;
using Shared.Core.Repositories;

namespace AMMS.VIETTEL.SMAS.Cores.Interfaces.Persons
{

    public interface IPersonRepository : IAsyncRepository<Person>
    {
        Task<Result<Person>> SaveAsync(Person data);
        Task<Result<PersonFace>> SaveImageAsync(string personId, string data, string folderName);
        Task<Result<PersonFace>> GetFacePersonById(string id);
    }

}
