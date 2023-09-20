using HiringService.Domain.Entities;

namespace HiringService.Application.Cache;

public static class RedisKeysPrefixes
{
    public static string CandidatePrefix { get; } = nameof(Candidate) + "_";

    public static string WorkerPrefix { get; } = nameof(Worker) + "_";

    public static string StagePrefix { get; } = nameof(HiringStage) + "_";

    public static string StageNamePrefix { get; } = nameof(HiringStageName) + "_";
}
