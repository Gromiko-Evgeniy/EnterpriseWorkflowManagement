using AutoMapper;
using ProjectManagementService.Application.DTOs;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Mapping.Profiles;
public class WorkerMappingProfile : Profile
{
    public WorkerMappingProfile()
    {
        CreateMap<NameEmailDTO, Worker>();
    }
}
