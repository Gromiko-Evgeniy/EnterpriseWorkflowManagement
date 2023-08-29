namespace ProjectManagementService.Application.DTOs.ProjectDTOs;

public class ProjectShortInfoDTO
{
    public string Id { get; set; }

    public string Objective { get; set; } = string.Empty;

    public string CustomerId { get; set; }

    public string LeadWorkerId { get; set; }
}
