using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProjectManagementService.Domain.Entities;
public class EntityWithId
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

}
