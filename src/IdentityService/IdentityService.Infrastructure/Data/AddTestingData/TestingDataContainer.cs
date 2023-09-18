namespace IdentityService.Infrastructure.Data.AddTestingData;

public static class TestingDataContainer
{
    public static async Task AddTestingData(DataContext context)
    {
        await CandidateTestingDataContainer.AddCandidateTestingData(context);

        await CustomerTestingDataContainer.AddCustomerTestingData(context);

        await WorkerTestingDataContainer.AddWorkerTestingData(context);
    }
}