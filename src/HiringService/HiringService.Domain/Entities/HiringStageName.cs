namespace HiringService.Domain.Entities;

public class HiringStageName
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Index { get; set; }

    public List<HiringStage> HiringStages { get; set; }
}