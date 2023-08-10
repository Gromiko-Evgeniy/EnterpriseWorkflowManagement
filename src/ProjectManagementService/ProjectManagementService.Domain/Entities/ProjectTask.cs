using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ProjectManagementService.Domain.Enumerations;

namespace ProjectManagementService.Domain.Entities;

[BsonIgnoreExtraElements]
public class ProjectTask
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("projectId")]
    public string ProjectId { get; set; }

    [BsonElement("priority")]
    public Priority Priority { get; set; } = Priority.Low;

    [BsonElement("status")]
    public ProjectTaskStatus Status { get; set; } = ProjectTaskStatus.ToDo;

    [BsonElement("startTime")]
    public DateTime StartTime { get; set; }

    [BsonElement("finishTime")]
    public DateTime FinishTime { get; set; }
}
