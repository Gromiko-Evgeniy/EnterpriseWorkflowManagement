namespace ProjectManagementService.Domain.Enumerations;

public enum ProjectTaskStatus
{
    ToDo = 1,
    InProgress,
    ReadyToApprove,
    Editing,
    Approved,
    Canceled
}