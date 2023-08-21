using IdentityService.Application.DTOs;
using IdentityService.Application.DTOs.CustomerDTOs;
using IdentityService.Application.Exceptions;
using IdentityService.Application.Exceptions.Customer;
using IdentityService.Application.RepositoryAbstractions;
using IdentityService.Application.ServiceAbstractions;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Services.EntityServices;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<LogInData> AddAsync(AddCustomerDTO customerDTO)
    {
        var oldCustomer = await _customerRepository.GetByEmailAsync(customerDTO.Email);

        if (oldCustomer is not null) throw new CustomerAlreadyExistsException();

        //send data to pm service?

        var newCustomer = new Customer()
        {
            Email = customerDTO.Email,
            Password = customerDTO.Password
        };

        _customerRepository.Add(newCustomer);
        await _customerRepository.SaveChangesAsync();

        return new LogInData() { Email = newCustomer.Email, Password = newCustomer.Password };
    }

    public async Task<Customer> GetByEmailAndPasswordAsync(LogInData data)
    {
        var customer = await _customerRepository.GetByEmailAsync(data.Email);

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
