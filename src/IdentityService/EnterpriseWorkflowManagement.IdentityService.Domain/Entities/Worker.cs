﻿namespace EnterpriseWorkflowManagement.IdentityService.Domain.Entities;

public class Worker
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public Position Position { get; set; }
}

public enum Position
{
    Candidate = 1,
    Worker,
    ProjectLeader,
    DepartmentHead
}
 