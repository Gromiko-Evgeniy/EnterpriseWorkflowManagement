using AutoMapper;
using IdentityService.Application.DTOs;
using IdentityService.Application.DTOs.CandidateDTO;
using IdentityService.Application.DTOs.CandidateDTOs;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Mapping.Profiles;

public class CandidateMappingProfile : Profile
{
    public CandidateMappingProfile()
    {
        CreateMap<AddCandidateDTO, Candidate>();
        CreateMap<Candidate, LogInData>();
        CreateMap<AddCandidateDTO, CandidateMessageDTO>();
    }
}
