using Hangfire;
using IdentityService.Application.RepositoryAbstractions;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.HangfireExtensions;
public static class WorkersActivityCheck
{
    public static async Task CheckWorkersActivity(IWorkerRepository workerRepository)
    {
        var today = DateTime.Now;
        var badWorkers = new List<Worker>();

        if (today.DayOfWeek == DayOfWeek.Saturday || today.DayOfWeek == DayOfWeek.Sunday)
        {
            var workers = await workerRepository.GetAllAsync();
            badWorkers = workers.Where(worker => !HadBeenWorkingToday(worker.LastTimeVisited)).ToList();
        }
        
        if (badWorkers.Count != 0)
        {
            foreach (var worker in badWorkers)
            {
                // send email "Are you OK?"
            }
        }
    }

    public static void AddCheckWorkersActivityRecurringJob(
        IWorkerRepository workerRepository, IRecurringJobManager recurringJobManager)
    {
        recurringJobManager.AddOrUpdate("myRecurringJobId", () => CheckWorkersActivity(workerRepository), Cron.Daily);
    }


    private static bool HadBeenWorkingToday(DateTime lastTimeVisited)
    {
        var WorkDayBegining = DateTime.Now.Date + new TimeSpan(6, 0, 0);

        return DateTime.Compare(lastTimeVisited, WorkDayBegining) > 0;
    }
}
