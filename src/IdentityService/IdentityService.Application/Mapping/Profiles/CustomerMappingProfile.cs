using AutoMapper;
using IdentityService.Application.DTOs.CustomerDTOs;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Mapping.Profiles;

public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<AddCustomerDTO, Customer>();
    }
}
