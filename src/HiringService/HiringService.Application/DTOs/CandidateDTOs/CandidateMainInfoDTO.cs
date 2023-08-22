namespace HiringService.Application.DTOs.CandidateDTOs;

public class CandidateMainInfoDTO
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string CV { get; set; } = string.Empty;

    public DateTime NextStageTime { get; set; }
}
