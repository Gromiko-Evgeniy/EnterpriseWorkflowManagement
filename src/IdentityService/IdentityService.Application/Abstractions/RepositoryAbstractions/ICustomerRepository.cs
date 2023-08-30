using IdentityService.Application.Abstractions.RepositoryAbstractions;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.RepositoryAbstractions;

public interface ICustomerRepository : IGenericRepository<Customer> { }
