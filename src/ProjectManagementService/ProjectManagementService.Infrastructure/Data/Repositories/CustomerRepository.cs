using ProjectManagementService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ProjectManagementService.Application.Abstractions;
using HiringService.Infrastructure.Data.Repositories;

namespace ProjectManagementService.Infrastucture.Data.Repositories;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(IConfiguration configuration, IMongoClient mongoClient) : 
        base(configuration, mongoClient, "CustomerCollectionName") { }

    public async new Task<string> AddAsync(Customer customer)
    {
        await base.AddAsync(customer);

        return customer.Id;
    }
}
