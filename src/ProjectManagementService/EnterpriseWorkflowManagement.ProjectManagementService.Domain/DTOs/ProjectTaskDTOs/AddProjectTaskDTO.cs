using EnterpriseWorkflowManagement.ProjectManagementService.Domain.Entities;

namespace EnterpriseWorkflowManagement.ProjectManagementService.Domain.DTOs.ProjectTaskDTOs;
public class AddProjectTaskDTO
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public Priority Priority { get; set; }

    public int ProjectId { get; set; }

}
