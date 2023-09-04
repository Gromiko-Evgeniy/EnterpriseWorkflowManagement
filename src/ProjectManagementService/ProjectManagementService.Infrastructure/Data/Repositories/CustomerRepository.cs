using ProjectManagementService.Domain.Entities;
using MongoDB.Driver;
using HiringService.Infrastructure.Data.Repositories;
using ProjectManagementService.Application.Configuration;
using Microsoft.Extensions.Options;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;

namespace ProjectManagementService.Infrastucture.Data.Repositories;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(IMongoClient mongoClient,
        IOptions<MongoDBConfiguration> DBconfiguration) : 
        base(mongoClient, DBconfiguration)
    { }
}
