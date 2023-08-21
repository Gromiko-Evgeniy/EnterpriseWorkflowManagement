using AutoMapper;
using IdentityService.Application.DTOs.WorkerDTOs;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Mapping.Profiles;
public class WorkerMappingProfile : Profile
{
    public WorkerMappingProfile()
    {
        CreateMap<AddWorkerDTO, Worker>();
        CreateMap<Worker, GetWorkerDTO>();
    }
}
