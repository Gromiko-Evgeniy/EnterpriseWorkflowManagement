using AutoMapper;
using HiringService.Application.Cache;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.CQRS.CustomerCommands;

public class AddCustomerHandler : IRequestHandler<AddCustomerCommand, string>
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IDistributedCache _cache;
    private readonly IMapper _mapper;

    public AddCustomerHandler(ICustomerRepository customerRepository,
        IDistributedCache cache, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<string> Handle(AddCustomerCommand request, CancellationToken cancellationToken)
    {
        var customerDTO = request.NameEmailDTO;

        var newCustomer = _mapper.Map<Customer>(customerDTO);

        string id = await _customerRepository.AddAsync(newCustomer);
        
        var emailKey = "Customer_" + newCustomer.Email;
        await _cache.SetRecordAsync(emailKey, newCustomer);

        return id;
    }
}
