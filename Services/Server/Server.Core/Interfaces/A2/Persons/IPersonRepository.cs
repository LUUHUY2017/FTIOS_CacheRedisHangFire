using AMMS.Core.Repositories;
using Server.Core.Entities.A0;
using Server.Core.Entities.A2;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.Persons
{

    public interface IPersonRepository : IAsyncRepository<A2_Person>
    {
        Task<Result<A2_Person>> SaveAsync(A2_Person data);
        Task<Result<A2_PersonFace>> SaveImageAsync(string personId, string data);
    }

}
