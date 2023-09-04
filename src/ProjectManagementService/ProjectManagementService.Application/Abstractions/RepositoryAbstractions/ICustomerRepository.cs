using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Abstractions.RepositoryAbstractions;

public interface ICustomerRepository : IGenericRepository<Customer>
{
    public new Task<string> AddAsync(Customer customer);
}
