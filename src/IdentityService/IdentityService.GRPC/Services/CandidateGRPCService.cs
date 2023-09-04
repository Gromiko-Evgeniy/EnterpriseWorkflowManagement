using Grpc.Core;
using IdentityService.Application.DTOs;
using IdentityService.Application.Exceptions.Worker;
using IdentityService.Application.RepositoryAbstractions;
using IdentityService.Application.TokenAbstractions;
using IdentityService.Domain.Entities;

namespace IdentityService.GRPC.Services;
public class CandidateGRPCService : CandidateService.CandidateServiceBase
{
    private readonly ICandidateRepository _candidateRepository;
    private readonly IWorkerRepository _workerRepository;
    private readonly IWorkerTokenService _tokenService;

    public CandidateGRPCService(ICandidateRepository candidateRepository,
        IWorkerRepository workerRepository, IWorkerTokenService tokenService)
    {
        _candidateRepository = candidateRepository;
        _workerRepository = workerRepository;
        _tokenService = tokenService;
    }
    public async override Task<NewJWTReply> RemoveCandidate(RemoveCandidateRequest request, ServerCallContext context)
    {
        Candidate candidateToRemove = null;
        try
        {
            candidateToRemove = await _candidateRepository.GetFirstAsync(candidate => candidate.Email == request.Email);
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync("\n\n\n" + ex.Message + "\n\n\n");
        }

        if (candidateToRemove is null)
        {
            return new NewJWTReply { NewJWT = "error: incorrect email" };
        }

        _candidateRepository.Remove(candidateToRemove);
        await _candidateRepository.SaveChangesAsync();

        var newWorker = new Worker()
        {
            Email = candidateToRemove.Email,
            Password = candidateToRemove.Password,
            Name = request.Name
        };

        try
        {
            _workerRepository.Add(newWorker);
            await _workerRepository.SaveChangesAsync();
        }
        catch (WorkerAlreadyExistsException ex)
        {
            return new NewJWTReply { NewJWT = "error: " + ex.Message };
        }

        var workerData = new LogInData() { Email = newWorker.Email, Password = newWorker.Password };
        var workerToken = await _tokenService.GetTokenAsync(workerData);

        return new NewJWTReply { NewJWT = workerToken };
    }
}
