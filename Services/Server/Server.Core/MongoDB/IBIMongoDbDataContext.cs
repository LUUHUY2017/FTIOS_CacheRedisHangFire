using MongoDB.Driver;
using Server.Core.MongoDB.Entities;

namespace Server.Core.MongoDB;

public interface IBIMongoDbDataContext
{
    IMongoCollection<POC_DataInOutEvent> POC_DataInOutEvents { get; }
}