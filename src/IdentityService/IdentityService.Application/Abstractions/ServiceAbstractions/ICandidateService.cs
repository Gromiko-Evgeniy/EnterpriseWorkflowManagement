using IdentityService.Application.Abstractions.ServiceAbstractions;
using IdentityService.Application.DTOs.CandidateDTO;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.ServiceAbstractions;

public interface ICandidateService : IGenericService<Candidate, AddCandidateDTO> { }
