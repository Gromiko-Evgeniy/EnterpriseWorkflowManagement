using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Data.AddTestingData;

public static class CustomerTestingDataContainer
{
    public static async Task AddCustomerTestingData(DataContext context)
    {
        if (!context.Customers.Any())
        {
            var Customer1 = new Customer()
            {
                Email = "customer1@gmail.com",
                Password = "1234",
            };
            
            var Customer2 = new Customer()
            {
                Email = "customer2@gmail.com",
                Password = "1234",
            };

            var testCustomers = new Customer[] { Customer1, Customer2 };

            context.Customers.AddRange(testCustomers);
            await context.SaveChangesAsync();
        }
    }
}