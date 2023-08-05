namespace EnterpriseWorkflowManagement.ProjectManagementService.Domain.Entities;

public class ProjectTask
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Priority Priority { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime FinishTime { get; set; }

    public Project Project { get; set; }

    public int ProjectId { get; set; }

    public ProjectTaskStatus Status { get; set; }

    public int StatusId { get; set; }
}

public enum Priority
{
    Low = 1,
    Medium,
    High
}

public enum ProjectTaskStatus
{
    ToDo = 1,
    InProgress,
    ReadyToApprove,
    Editing,
    Approved,
    Canceled
}
