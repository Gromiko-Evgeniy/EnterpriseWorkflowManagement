using ProjectManagementService.Domain.Enumerations;

namespace ProjectManagementService.Application.ProjectTaskDTOs;

public class AddProjectTaskDTO
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Priority Priority { get; set; }

    public string ProjectId { get; set; }
}
