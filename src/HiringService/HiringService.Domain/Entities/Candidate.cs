namespace HiringService.Domain.Entities;

public class Candidate
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string CV { get; set; } = string.Empty;

    public DateTime NextStageTime { get; set; }

    public List<HiringStage> HiringStages { get; set; }
}
