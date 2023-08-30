using HiringService.Application.Abstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Abstractions;

public interface ICustomerRepository : IGenericRepository<Customer>
{
    public new Task<string> AddAsync(Customer customer);
}
