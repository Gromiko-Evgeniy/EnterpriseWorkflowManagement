using ProjectManagementService.Domain.Enumerations;

namespace ProjectManagementService.Application.TaskDTOs;

public class TaskWithWorkerEmailDTO
{
    public string Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ProjectId { get; set; }

    public Priority Priority { get; set; } = Priority.Low;

    public ProjectTaskStatus Status { get; set; } = ProjectTaskStatus.ToDo;

    public DateTime StartTime { get; set; }

    public DateTime FinishTime { get; set; }

    public string? WorkerEmail { get; set; }
}
