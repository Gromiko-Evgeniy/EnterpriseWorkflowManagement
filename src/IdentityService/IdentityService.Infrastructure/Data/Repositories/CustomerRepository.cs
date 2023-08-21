using IdentityService.Application.RepositoryAbstractions;
using IdentityService.Domain.Entities;

namespace IdentityService.Infrastructure.Data.Repositories;

public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(DataContext context) : base(context) { }

    public async override Task<Customer?> GetByEmailAsync(string email)
    {
        return await GetFirstAsync(s => s.Email == email);
    }
}
