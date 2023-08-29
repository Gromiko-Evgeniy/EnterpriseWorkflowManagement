namespace HiringService.Domain.Entities;

public class HiringStage
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public bool PassedSuccessfully { get; set; } = false;

    public DateTime DateTime { get; set; }

    public Candidate Candidate { get; set; }

    public int CandidateId { get; set; }

    public Worker Intervier { get; set; }

    public int IntervierId { get; set; }

    public HiringStageName HiringStageName { get; set; }

    public int HiringStageNameId { get; set; }
}
