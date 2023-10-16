using AutoMapper;
using ProjectManagementService.Application.TaskDTOs;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Mapping.Profiles;

public class ProjectTaskMappingProfile : Profile
{
    public ProjectTaskMappingProfile()
    {
        CreateMap<AddProjectTaskDTO, ProjectTask>();
        CreateMap<ProjectTask, TaskShortInfoDTO>();
        CreateMap<ProjectTask, TaskMainInfoDTO>();
        CreateMap<ProjectTask, TaskWithWorkerEmailDTO>();
    }
}
