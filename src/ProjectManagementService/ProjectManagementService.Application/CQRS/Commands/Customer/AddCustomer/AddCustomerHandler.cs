using AutoMapper;
using MediatR;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.CustomerCommands;

public class AddCustomerHandler : IRequestHandler<AddCustomerCommand, string>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;

    public AddCustomerHandler(ICustomerRepository customerRepository, IMapper mapper)
    {
        _mapper = mapper;
        _customerRepository = customerRepository;
    }

    public async Task<string> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerDTO = request.NameEmailDTO;

        var newCustomer = _mapper.Map<Customer>(customerDTO);

        string id = await _customerRepository.AddAsync(newCustomer);

        return id;
    }
}
