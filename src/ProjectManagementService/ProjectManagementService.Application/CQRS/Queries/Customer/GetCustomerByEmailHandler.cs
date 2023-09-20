using MediatR;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.Exceptions.Customer;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.CustomerQueries;

public class GetCustomerByEmailHandler : IRequestHandler<GetCustomerByEmailQuery, Customer>
{
    private readonly ICustomerRepository _customerRepository;

    public GetCustomerByEmailHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<Customer> Handle(GetCustomerByEmailQuery request, CancellationToken cancellationToken)
    {
        var candidate = await _customerRepository.
            GetFirstAsync(customer => customer.Email == request.Email);

        if (candidate is null) throw new NoCustomerWithSuchEmailException();

        return candidate;
    }
}