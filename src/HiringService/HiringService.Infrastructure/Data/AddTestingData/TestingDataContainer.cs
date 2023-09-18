using HiringService.Domain.Entities;

namespace HiringService.Infrastructure.Data.AddTestingData;

public static class TestingDataContainer
{
    public static async Task AddTestingData(DataContext context)
    {
        CreateTestCandidates(out Candidate Candidate1, out Candidate Candidate2);

        CreateTestWorkers(out Worker Worker1, out Worker Worker2);

        CreateTestHiringStageNames(out HiringStageName Prescreen, out HiringStageName English,
            out HiringStageName TechnicalInterview, out HiringStageName FinalInterview);

        if (!context.Candidates.Any())
        {
            var defaultCandidates = new Candidate[] { Candidate1, Candidate2 };

            context.Candidates.AddRange(defaultCandidates);
            await context.SaveChangesAsync();
        }

        if (!context.Workers.Any())
        {
            var defaultWorkers = new Worker[] { Worker1, Worker2 };

            context.Workers.AddRange(defaultWorkers);
            await context.SaveChangesAsync();
        }

        if (!context.HiringStageNames.Any())
        {
            var defaultStageNames = new HiringStageName[] { Prescreen, English, TechnicalInterview, FinalInterview };

            context.HiringStageNames.AddRange(defaultStageNames);
            await context.SaveChangesAsync();
        }

        if (!context.HiringStages.Any())
        {
            var HiringStage1 = new HiringStage()
            {
                Description = "Prescreen was OK",
                PassedSuccessfully = true,
                DateTime = new DateTime(2023, 8, 20, 15, 0, 0),
                Candidate = Candidate1,
                Intervier = Worker1,
                HiringStageName = Prescreen
            };

            var HiringStage2 = new HiringStage()
            {
                Description = "Candidate has B2 level",
                PassedSuccessfully = true,
                DateTime = new DateTime(2023, 8, 23, 15, 0, 0),
                Candidate = Candidate1,
                Intervier = Worker1,
                HiringStageName = English
            };

            var HiringStage3 = new HiringStage()
            {
                Description = "Candidate has good tech knowledge",
                PassedSuccessfully = true,
                DateTime = new DateTime(2023, 9, 2, 13, 0, 0),
                Candidate = Candidate1,
                Intervier = Worker2,
                HiringStageName = TechnicalInterview
            };

            var HiringStage4 = new HiringStage()
            {
                Description = "Prescreen was OK",
                PassedSuccessfully = true,
                DateTime = new DateTime(2023, 9, 20, 15, 0, 0),
                Candidate = Candidate2,
                Intervier = Worker2,
                HiringStageName = Prescreen
            };
        }
    }

    private static void CreateTestCandidates(out Candidate Candidate1, out Candidate Candidate2)
    {
        Candidate1 = new Candidate()
        {
            Name = "Candidate1",
            Email = "candidate1@gmail.com",
            CV = "i am candidate1",
        };

        Candidate2 = new Candidate()
        {
            Name = "Candidate2",
            Email = "candidate2@gmail.com",
            CV = "i am candidat2",
        };
    }

    private static void CreateTestWorkers(out Worker Worker1, out Worker Worker2)
    {
        Worker1 = new Worker()
        {
            Name = "worker1",
            Email = "worker1@gmail.com"
        };

        Worker2 = new Worker()
        {
            Name = "worker2",
            Email = "worker2@gmail.com"
        };
    }

    private static void CreateTestHiringStageNames(
        out HiringStageName Prescreen, out HiringStageName English,
        out HiringStageName TechnicalInterview, out HiringStageName FinalInterview)
    {
        Prescreen = new HiringStageName()
        {
            Name = "Prescreen",
            Index = 0
        };

        English = new HiringStageName()
        {
            Name = "English",
            Index = 1
        };

        TechnicalInterview = new HiringStageName()
        {
            Name = "Technical interview",
            Index = 2
        };

        FinalInterview = new HiringStageName()
        {
            Name = "Final interview",
            Index = 3
        };
    }
}
