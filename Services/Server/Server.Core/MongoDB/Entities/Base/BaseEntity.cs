using MongoDB.Bson.Serialization.Attributes;

namespace Server.Core.MongoDB.Entities.Base;

public class BaseEntity
{
    [BsonId]
    public string Id { get; set; }
}