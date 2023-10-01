using AutoMapper;
using ProjectManagementService.Application.ProjectDTOs;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.Application.Mapping.Profiles;

public class ProjectMappingProfile : Profile
{
    public ProjectMappingProfile()
    {
        CreateMap<AddProjectDTO, Project>();
        CreateMap<Project, ProjectShortInfoDTO>();
        CreateMap<Project, ProjectMainInfoDTO>();
        CreateMap<Project, ProjectWithTaskListDTO>();
        CreateMap<ProjectMainInfoDTO, ProjectWithTaskListDTO>();
    }
}
