using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Server.Core.MongoDB;
using Server.Core.MongoDB.Entities;
 
namespace Server.Infrastructure.MongoDB;

public class BIMongoDbDataContext : IBIMongoDbDataContext
{
    public IMongoCollection<POC_DataInOutEvent> POC_DataInOutEvents { get; }


    public BIMongoDbDataContext(IConfiguration configuration)
    {
        //var client = new MongoClient(configuration.GetSection("MongoDatabaseSettings:ConnectionString").Value);
        //var database = client.GetDatabase(configuration.GetSection("MongoDatabaseSettings:DatabaseName").Value);

        var client = new MongoClient(configuration.GetSection("MongoDatabaseSettings:ConnectionString").Value);
        var database = client.GetDatabase(configuration.GetSection("MongoDatabaseSettings:DatabaseName").Value);

        POC_DataInOutEvents = database.GetCollection<POC_DataInOutEvent>("POC_DataInOutEvents");
    }
}