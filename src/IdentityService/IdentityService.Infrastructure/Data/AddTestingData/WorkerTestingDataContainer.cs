using IdentityService.Domain.Entities;
using IdentityService.Domain.Enumerations;

namespace IdentityService.Infrastructure.Data.AddTestingData;

public static class WorkerTestingDataContainer
{
    public static async Task AddWorkerTestingData(DataContext context)
    {
        if (!context.Workers.Any())
        {
            var Worker1 = new Worker()
            {
                Name = "worker1",
                Email = "worker1@gmail.com",
                Password = "1234",
                Position = Position.Worker,
            };

            var Worker2 = new Worker()
            {
                Name = "worker2",
                Email = "worker2@gmail.com",
                Password = "1234",
                Position = Position.Worker,
            };

            var ProjectLeader = new Worker()
            {
                Name = "ProjectLeader",
                Email = "plead@gmail.com",
                Password = "1234",
                Position = Position.ProjectLeader,
            };

            var DepartmentHead = new Worker()
            {
                Name = "DepartmentHead",
                Email = "dephead@gmail.com",
                Password = "1234",
                Position = Position.DepartmentHead,
            };

            var testWorkers = new Worker[] { Worker1, Worker2 };

            context.Workers.AddRange(testWorkers);
            await context.SaveChangesAsync();
        }
    }
}