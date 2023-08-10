using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Abstractions;

public interface ICustomersRepository
{
    public Task AddAsync(Customer customer);

    public Task RemoveAsync(string customerId);
}