using AutoMapper;
using HiringService.Application.DTOs.CandidateDTOs;
using HiringService.Domain.Entities;

namespace HiringService.Application.Mapping.Profiles;

public class CandidateMappingProfile : Profile
{
    public CandidateMappingProfile()
    {
        CreateMap<AddCandidateDTO, Candidate>();
        CreateMap<Candidate, CandidateMainInfoDTO>();
        CreateMap<Candidate, CandidateShortInfoDTO>();
    }
}
