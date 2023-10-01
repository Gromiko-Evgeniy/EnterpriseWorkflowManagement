namespace ProjectManagementService.Application.ProjectDTOs;

public class UpdateProjectDTO
{
    public string Id { get; set; }

    public string Objective { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string JWT { get; set; } = string.Empty;
}
