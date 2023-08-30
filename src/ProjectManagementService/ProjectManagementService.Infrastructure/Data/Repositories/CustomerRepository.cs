using ProjectManagementService.Domain.Entities;
using MongoDB.Driver;
using ProjectManagementService.Application.Abstractions;
using HiringService.Infrastructure.Data.Repositories;
using ProjectManagementService.Application.Configuration;
using Microsoft.Extensions.Options;

namespace ProjectManagementService.Infrastucture.Data.Repositories;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(IMongoClient mongoClient,
        IOptions<MongoDBConfiguration> DBconfiguration) : 
        base(mongoClient, DBconfiguration)
    { }
}
