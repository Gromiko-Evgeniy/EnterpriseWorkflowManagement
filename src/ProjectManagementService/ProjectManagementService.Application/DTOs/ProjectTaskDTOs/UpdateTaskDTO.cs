namespace ProjectManagementService.Application.TaskDTOs;

public class UpdateTaskDTO
{
    public string Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string JWT { get; set; } = string.Empty;
}
