using AutoMapper;
using ProjectManagementService.Application.DTOs;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Mapping.Profiles;

public class CustomerMappingProfile : Profile
{
    public CustomerMappingProfile()
    {
        CreateMap<NameEmailDTO, Customer>();
    }
}