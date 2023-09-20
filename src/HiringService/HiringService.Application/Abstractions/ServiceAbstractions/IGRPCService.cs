namespace HiringService.Application.Abstractions.ServiceAbstractions;

public interface IGRPCService
{
    public Task<string> DeleteCandidate(string email, string name);
}
