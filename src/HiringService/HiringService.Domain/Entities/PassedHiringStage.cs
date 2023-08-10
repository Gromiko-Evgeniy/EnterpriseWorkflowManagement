namespace HiringService.Domain.Projects;

public class PassedHiringStage
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public bool PassedSuccessfully { get; set; } = false;

    public DateTime DateTime { get; set; }

    public Candidate Candidate { get; set; }

    public int CandidateId { get; set; }

    public Worker Intervier { get; set; }

    public int IntervierId { get; set; }

    public HiringStage HiringStage { get; set; }

    public int HiringStageId { get; set; }
}

