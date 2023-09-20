using Grpc.Net.Client;
using HiringService.Application.Abstractions.ServiceAbstractions;
using IdentityService.GRPC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static IdentityService.GRPC.CandidateService;
using Microsoft.Extensions.Options;

namespace HiringService.Application.Services;

internal class GRPCService : IGRPCService
{
    private readonly CandidateServiceClient _client;

    public GRPCService(IServiceProvider serviceProvider)
    {
        _client = serviceProvider.GetRequiredService<CandidateServiceClient>();
    }

    public async Task<string> DeleteCandidate(string email, string name)
    {
        var deleteRequest = new RemoveCandidateRequest() { Email = email, Name = name };

        var newJWT = await _client.RemoveCandidateAsync(deleteRequest);

        return newJWT.NewJWT;
    }
}
