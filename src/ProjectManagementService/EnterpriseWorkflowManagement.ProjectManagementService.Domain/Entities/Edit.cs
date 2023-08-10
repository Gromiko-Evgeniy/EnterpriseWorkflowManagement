using ProjectManagementService.Domain.Enumerations;

namespace ProjectManagementService.Domain.Entities;

public class Edit
{
    public int Id { get; set; }

    public string Text { get; set; } = string.Empty;

    public ProjectTaskStatus Status { get; set; }

    public int StatusId { get; set; }
}