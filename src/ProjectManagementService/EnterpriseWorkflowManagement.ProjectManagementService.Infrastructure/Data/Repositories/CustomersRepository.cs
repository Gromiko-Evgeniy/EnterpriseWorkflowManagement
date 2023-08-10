using ProjectManagementService.Domain.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using ProjectManagementService.Application.Abstractions;

namespace ProjectManagementService.Infrastucture.Data.Repositories;
public class CustomersRepository : ICustomersRepository
{
    private readonly IMongoCollection<Customer> customers;

    public CustomersRepository(IConfiguration configuration, IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase(configuration["DBConnectionSettings:DatabaseName"]);
        customers = database.GetCollection<Customer>(configuration["DBConnectionSettings:Collections:CustomerCollectionName"]);
    }

    public async Task AddAsync(Customer customer)
    {
        await customers.InsertOneAsync(customer);
    } 

    public async Task RemoveAsync(string customerId)
    {
        await customers.DeleteOneAsync(customer => customer.Id == customerId);
    }
}
