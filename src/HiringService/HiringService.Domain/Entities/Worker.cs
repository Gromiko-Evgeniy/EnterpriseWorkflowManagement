namespace HiringService.Domain.Entities;

public class Worker
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public List<HiringStage> HiringStages { get; set; }
}
