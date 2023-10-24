using AutoMapper;
using HiringService.Application.DTOs.StageNameDTOs;
using HiringService.Domain.Entities;

namespace HiringService.Application.Mapping.Profiles;

public class StageNameMappingProfile : Profile
{
    public StageNameMappingProfile()
    {
        CreateMap<HiringStageName, GetStageNameDTO>();
        CreateMap<GetStageNameDTO, GetStageNameDTO>();
        CreateMap<AddStageNameDTO, HiringStageName>();
    }
}
