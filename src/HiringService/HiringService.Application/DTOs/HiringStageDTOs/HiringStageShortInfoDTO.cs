namespace HiringService.Application.DTOs.HiringStageDTOs;

public class HiringStageShortInfoDTO
{
    public int Id { get; set; }

    public string Description { get; set; } = string.Empty;

    public bool PassedSuccessfully { get; set; } = false;

    public DateTime DateTime { get; set; }
}
