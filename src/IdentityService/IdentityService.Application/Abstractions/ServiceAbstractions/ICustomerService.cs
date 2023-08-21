using IdentityService.Application.Abstractions.ServiceAbstractions;
using IdentityService.Application.DTOs.CustomerDTOs;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.ServiceAbstractions;

public interface ICustomerService : IGenericService<Customer, AddCustomerDTO> { }
