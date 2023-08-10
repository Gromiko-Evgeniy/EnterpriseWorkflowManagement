using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using ProjectManagementService.Domain.Enumerations;

namespace ProjectManagementService.Domain.Entities;

[BsonIgnoreExtraElements]
public class Project
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("objective")]
    public string Objective { get; set; } = string.Empty;

    [BsonElement("description")]
    public string Description { get; set; } = string.Empty;

    [BsonElement("customerId")]
    public string CustomerId { get; set; }

    [BsonElement("leadWorkerId")]
    public string LeadWorkerId { get; set; }

    [BsonElement("status")]
    public ProjectStatus Status { get; set; } = ProjectStatus.WaitingToStart;
}