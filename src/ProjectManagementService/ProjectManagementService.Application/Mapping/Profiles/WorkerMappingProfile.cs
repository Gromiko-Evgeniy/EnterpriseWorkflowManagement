using AutoMapper;
using ProjectManagementService.Application.DTOs.WorkerDTOs;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Mapping.Profiles;
public class WorkerMappingProfile : Profile
{
    public WorkerMappingProfile()
    {
        CreateMap<AddWorkerDTO, Worker>();
    }
}
