﻿using AMMS.Core.Repositories;
using Server.Core.Entities.A0;
using Server.Core.Entities.A2;
using Shared.Core.Commons;

namespace Server.Core.Interfaces.A2.Persons
{

    public interface IPersonRepository : IAsyncRepository<Person>
    {
        Task<Result<Person>> SaveAsync(Person data);
        Task<Result<PersonFace>> SaveImageAsync(string personId, string orgId, string folderName);
        Task<Result<PersonFace>> GetFacePersonById(string id);
    }

}
