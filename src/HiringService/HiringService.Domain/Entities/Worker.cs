namespace HiringService.Domain.Projects;

public class Worker
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public PassedHiringStage PassedHiringStage { get; set; }

    public int PassedHiringStageId { get; set; }
}
