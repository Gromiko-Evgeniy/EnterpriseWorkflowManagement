namespace ProjectManagementService.Application.TaskDTOs;

public class TaskShortInfoDTO
{
    public string Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ProjectId { get; set; }
}
