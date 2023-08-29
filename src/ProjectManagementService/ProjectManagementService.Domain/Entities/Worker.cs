using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ProjectManagementService.Domain.Entities;

[BsonIgnoreExtraElements]
public class Worker : EntityWithId
{
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("currentTaskId")]
    public string? CurrentTaskId { get; set; } = null;

    [BsonElement("currentProjectId")]
    public string? CurrentProjectId { get; set; }
}
