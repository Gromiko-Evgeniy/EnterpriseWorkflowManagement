namespace HiringService.Domain.Projects
{
    public class HiringStage
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<PassedHiringStage> PassedHiringStages { get; set; }
    }
}
