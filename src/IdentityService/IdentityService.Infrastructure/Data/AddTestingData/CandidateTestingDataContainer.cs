using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Data.AddTestingData;

public static class CandidateTestingDataContainer
{
    public static async Task AddCandidateTestingData(DataContext context)
    {
        if (!context.Candidates.Any())
        {
            var Candidate1 = new Candidate()
            {
                Email = "candidate1@gmail.com",
                Password = "1234",
            };

            var Candidate2 = new Candidate()
            {
                Email = "candidate2@gmail.com",
                Password = "1234",
            };

            var testCandidates = new Candidate[] { Candidate1, Candidate2 };

            context.Candidates.AddRange(testCandidates);
            await context.SaveChangesAsync();
        }
    }
}