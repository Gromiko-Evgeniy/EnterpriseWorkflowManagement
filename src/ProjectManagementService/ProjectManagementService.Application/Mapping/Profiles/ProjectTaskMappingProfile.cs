using AutoMapper;
using ProjectManagementService.Application.ProjectTaskDTOs;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Mapping.Profiles;

public class ProjectTaskMappingProfile : Profile
{
    public ProjectTaskMappingProfile()
    {
        CreateMap<AddProjectTaskDTO, ProjectTask>();
    }
}
