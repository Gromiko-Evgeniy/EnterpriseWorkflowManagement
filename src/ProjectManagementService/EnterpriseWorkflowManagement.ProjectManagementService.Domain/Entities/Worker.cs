using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProjectManagementService.Domain.Entities;

[BsonIgnoreExtraElements]
public class Worker
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("currentTaskId")]
    public string? CurrentTaskId { get; set; } = null;

    [BsonElement("currentProjectId")]
    public string? CurrentProjectId { get; set; }
}
