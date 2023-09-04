using Grpc.Net.Client;
using HiringService.Application.Abstractions.ServiceAbstractions;
using IdentityService.GRPC;
using Microsoft.Extensions.Configuration;

namespace HiringService.Application.Services;

internal class GRPCService : IGRPCService
{
    private readonly string _serverAddress;

    public GRPCService(IConfiguration configuration)
    {
        _serverAddress = configuration["GRPC:IdentityServerAddress"];
    }

    public async Task<string> DeleteCandidate(string email, string name)
    {
        var channel = GrpcChannel.ForAddress(_serverAddress);

        var client = new CandidateService.CandidateServiceClient(channel);

        var deleteRequest = new RemoveCandidateRequest() { Email = email, Name = name };

        var newJWT = await client.RemoveCandidateAsync(deleteRequest);

        return newJWT.NewJWT;
    }
}
