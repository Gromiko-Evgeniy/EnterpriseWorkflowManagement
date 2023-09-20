using Grpc.Net.Client;
using HiringService.Application.Abstractions.ServiceAbstractions;
using IdentityService.GRPC;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace HiringService.Application.Services;

internal class GRPCService : IGRPCService
{
    private readonly string _serverAddress;

    public GRPCService(IConfiguration configuration, IOptions<IdentityGRPCServiceAddress> gRPCServiceAddress)
    {
        _serverAddress = gRPCServiceAddress.Value.IdentityServerAddress!;
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
