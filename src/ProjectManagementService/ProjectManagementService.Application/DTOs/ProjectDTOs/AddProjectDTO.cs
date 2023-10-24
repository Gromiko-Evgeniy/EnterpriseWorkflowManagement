namespace ProjectManagementService.Application.ProjectDTOs;

public class AddProjectDTO
{
    public string Objective { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string CustomerId { get; set; }
}
