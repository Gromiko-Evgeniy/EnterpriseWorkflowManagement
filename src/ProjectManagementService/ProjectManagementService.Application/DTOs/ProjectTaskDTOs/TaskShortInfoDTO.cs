namespace ProjectManagementService.Application.DTOs.ProjectTaskDTOs;

public class TaskShortInfoDTO
{
    public string Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ProjectId { get; set; }
}
