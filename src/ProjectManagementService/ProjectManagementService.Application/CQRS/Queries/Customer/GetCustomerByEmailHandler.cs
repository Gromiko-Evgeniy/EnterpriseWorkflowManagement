using HiringService.Application.Cache;
using MediatR;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Exceptions.Customer;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.CustomerQueries;

public class GetCustomerByEmailHandler : IRequestHandler<GetCustomerByEmailQuery, Customer>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IDistributedCache _cache;

    public GetCustomerByEmailHandler(ICustomerRepository customerRepository,
        IDistributedCache cache)
    {
        _customerRepository = customerRepository;
        _cache = cache;
    }

    public async Task<Customer> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
    {
        var emailKey = RedisKeysPrefixes.CustomerPrefix + request.Email;
        var customer = await _cache.GetRecordAsync<Customer>(emailKey);

        if (customer is null)
        {
            customer = await _customerRepository
                .GetFirstAsync(customer => customer.Email == request.Email);
        }

        if (customer is null) throw new NoCustomerWithSuchEmailException();

        await _cache.SetRecordAsync(emailKey, customer);

        return customer;
    }
}