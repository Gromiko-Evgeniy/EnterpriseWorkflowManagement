namespace EnterpriseWorkflowManagement.ProjectManagementService.Domain.Entities;

public class Worker
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public ProjectTask CurrentTask { get; set; }

    public int CurrentTaskId { get; set; }

    public Project CurrentProject { get; set; }

    public int CurrentProjectId { get; set; }
}
