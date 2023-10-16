using ProjectManagementService.Domain.Entities;

namespace HiringService.Application.Cache;

public static class RedisKeysPrefixes
{
    public static string CustomerPrefix { get; } = nameof(Customer) + "_";

    public static string WorkerPrefix { get; } = nameof(Worker) + "_";

    public static string ProjectPrefix { get; } = nameof(Project) + "_";

    public static string ProjectTaskPrefix { get; } = nameof(ProjectTask) + "_";
}
