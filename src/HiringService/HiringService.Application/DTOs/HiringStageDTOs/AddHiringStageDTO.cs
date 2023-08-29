namespace HiringService.Application.DTOs.HiringStageDTOs;

public class AddHiringStageDTO
{
    public string Description { get; set; } = string.Empty;

    public DateTime DateTime { get; set; }

    public int CandidateId { get; set; }

    public int IntervierId { get; set; }

    public int HiringStageNameId { get; set; }
}
