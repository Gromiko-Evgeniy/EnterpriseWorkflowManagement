using ProjectManagementService.Domain.Enumerations;

namespace ProjectManagementService.Application.DTOs.ProjectDTOs;

public class ProjectMainInfoDTO
{
    public string Objective { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string CustomerId { get; set; }

    public string LeadWorkerId { get; set; }

    public ProjectStatus Status { get; set; } = ProjectStatus.WaitingToStart;
}
