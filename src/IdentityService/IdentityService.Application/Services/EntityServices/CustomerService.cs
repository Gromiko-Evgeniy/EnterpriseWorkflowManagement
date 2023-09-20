using AutoMapper;
using IdentityService.Application.DTOs;
using IdentityService.Application.DTOs.CustomerDTOs;
using IdentityService.Application.Exceptions;
using IdentityService.Application.Exceptions.Customer;
using IdentityService.Application.KafkaAbstractions;
using IdentityService.Application.RepositoryAbstractions;
using IdentityService.Application.ServiceAbstractions;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Services.EntityServices;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IKafkaProducer _kafkaProducer;
    private readonly IMapper _mapper;

    public CustomerService(ICustomerRepository customerRepository,
        IMapper mapper, IKafkaProducer kafkaProducer)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _kafkaProducer = kafkaProducer;
    }

    public async Task<LogInData> AddAsync(AddCustomerDTO customerDTO)
    {
        var oldCustomer = await _customerRepository.
            GetFirstAsync(customer => customer.Email == customerDTO.Email);

        if (oldCustomer is not null) throw new CustomerAlreadyExistsException();

        var newCustomer = _mapper.Map<Customer>(customerDTO);

        _customerRepository.Add(newCustomer);
        await _customerRepository.SaveChangesAsync();

        _kafkaProducer.SendAddCustomerMessage(_mapper.Map<NameEmailDTO>(customerDTO));

        return _mapper.Map<LogInData>(newCustomer);
    }

    public async Task<Customer> GetByEmailAndPasswordAsync(LogInData data)
    {
        var customer = await _customerRepository.
            GetFirstAsync(customer => customer.Email == data.Email);

        if (customer is null) throw new NoCustomerWithSuchEmailException();
        if (customer.Password != data.Password) throw new IncorrectPasswordException();

        return customer;
    }

    public async Task UpdatePasswordAsync(string email, string prevPassword, string newPassword)
    {
        var findData = new LogInData() { Email = email, Password = prevPassword };

        var customer = await GetByEmailAndPasswordAsync(findData);

        customer.Password = newPassword;

        _customerRepository.Update(customer);
        await _customerRepository.SaveChangesAsync();
    }
}
