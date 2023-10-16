using ProjectManagementService.Application.TaskDTOs;
using ProjectManagementService.Domain.Enumerations;

namespace ProjectManagementService.Application.ProjectDTOs;

public class ProjectWithTaskListDTO
{
    public string Id { get; set; }

    public string Objective { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ProjectStatus Status { get; set; } = ProjectStatus.WaitingToStart;

    public List<TaskWithWorkerEmailDTO> Tasks { get; set; } = new List<TaskWithWorkerEmailDTO>();
}