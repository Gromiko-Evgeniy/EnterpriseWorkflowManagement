using AutoMapper;
using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Domain.Entities;

namespace HiringService.Application.Mapping.Profiles;
public class HiringStageMappingProfile : Profile
{
    public HiringStageMappingProfile()
    {
        CreateMap<AddHiringStageDTO, HiringStage>();
        CreateMap<HiringStage, HiringStageMainInfoDTO>();
        CreateMap<HiringStage, HiringStageShortInfoDTO>();
    }
}
