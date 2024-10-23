using AMMS.Core.Repositories;
using Server.Core.Entities.A2;

namespace Server.Core.Interfaces.A2.Persons
{

    public interface IPersonRepository : IAsyncRepository<A2_Person>
    {
    }
}
