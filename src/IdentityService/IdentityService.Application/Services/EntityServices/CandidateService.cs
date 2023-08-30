using AutoMapper;
using IdentityService.Application.DTOs;
using IdentityService.Application.DTOs.CandidateDTO;
using IdentityService.Application.Exceptions;
using IdentityService.Application.Exceptions.Candidate;
using IdentityService.Application.RepositoryAbstractions;
using IdentityService.Application.ServiceAbstractions;
using IdentityService.Domain.Entities;

namespace IdentityService.Application.Services.EntityServices;

public class CandidateService : ICandidateService
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IMapper _mapper;

    public CandidateService(ICandidateRepository candidateRepository, IMapper mapper)
    {
        _candidateRepository = candidateRepository;
        _mapper = mapper;
    }

    public async Task<LogInData> AddAsync(AddCandidateDTO candidateDTO)
    {
        var oldCandidate = await _candidateRepository.
            GetFirstAsync(candidate => candidate.Email == candidateDTO.Email);

        if (oldCandidate is not null) throw new NoCandidateWithSuchEmailException();

        //send data to hiring service?

        var newCandidate = _mapper.Map<Candidate>(candidateDTO);

        _candidateRepository.Add(newCandidate);
        await _candidateRepository.SaveChangesAsync();

        return _mapper.Map<LogInData>(newCandidate);
    }

    public async Task<Candidate> GetByEmailAndPasswordAsync(LogInData data)
    {
        var candidate = await _candidateRepository.
            GetFirstAsync(candidate => candidate.Email == data.Email);

        if (candidate is null) throw new NoCandidateWithSuchEmailException();
        if (candidate.Password != data.Password) throw new IncorrectPasswordException();

        return candidate;
    }

    public async Task UpdatePasswordAsync(string email, string prevPassword, string newPassword)
    {
        var findData = new LogInData() { Email = email, Password = prevPassword };

        var candidate = await GetByEmailAndPasswordAsync(findData);

        candidate.Password = newPassword;

        _candidateRepository.Update(candidate);
        await _candidateRepository.SaveChangesAsync();
    }
}
