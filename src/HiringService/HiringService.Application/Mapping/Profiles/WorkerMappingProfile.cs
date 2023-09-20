using AutoMapper;
using HiringService.Domain.Entities;
using ProjectManagementService.Application.DTOs;

namespace HiringService.Application.Mapping.Profiles;

public class WorkerMappingProfile : Profile
{
    public WorkerMappingProfile()
    {
        CreateMap<NameEmailDTO, Worker>();
    }
}
