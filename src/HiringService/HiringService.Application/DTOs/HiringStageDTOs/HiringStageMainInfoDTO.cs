namespace HiringService.Application.DTOs.HiringStageDTOs;

public class HiringStageMainInfoDTO
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public bool PassedSuccessfully { get; set; } = false;

    public DateTime DateTime { get; set; }

    public int CandidateId { get; set; }

    public int IntervierId { get; set; }

    public int HiringStageNameId { get; set; }
}
