using HiringService.Application.DTOs.CandidateDTOs;
using MediatR;

namespace HiringService.Application.CQRS.CandidateCommands;

public sealed record AddCandidateCommand(AddCandidateDTO CandidateDTO) : IRequest<int> { }
