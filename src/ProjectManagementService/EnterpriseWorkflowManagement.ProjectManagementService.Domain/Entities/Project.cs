namespace EnterpriseWorkflowManagement.ProjectManagementService.Domain.Entities;

public class Project
{
    public int Id { get; set; }

    public string Objective { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ProjectStatuses Status { get; set; }

    public Customer Customer { get; set; } 

    public int CustomerId { get; set; }

    public Worker LeadWorker { get; set; }

    public int LeadWorkerId { get; set; }
}

public enum ProjectStatuses
{
    WaitingToStart = 1,
    InProgress, 
    Done
}