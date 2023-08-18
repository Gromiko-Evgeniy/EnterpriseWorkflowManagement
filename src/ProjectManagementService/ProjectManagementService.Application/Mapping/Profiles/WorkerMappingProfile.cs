using AutoMapper;
using ProjectManagementService.Application.WorkerDTOs;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Mapping.Profiles;
public class WorkerMappingProfile : Profile
{
    public WorkerMappingProfile()
    {
        CreateMap<AddWorkerDTO, Worker>();
    }
}
