using IdentityService.Application.DTOs;
using IdentityService.Application.DTOs.CandidateDTOs;
using IdentityService.Application.KafkaAbstractions;
using Microsoft.Extensions.Configuration;

namespace IdentityService.Application.Services.Kafka;

internal class KafkaProducer : IKafkaProducer
{
    private readonly string _bootstrapServers;

    public KafkaProducer(IConfiguration configuration)
    {
        _bootstrapServers = configuration.GetSection("KafkaBootstrapServer").Get<string>();
    }

    public void SendAddWorkerMessage(NameEmailDTO data)
    {
        KafkaProducerBaseFunctionality.SendMessage(_bootstrapServers, "add-workers", data);
    }

    public void SendRemoveWorkerMessage(string email)
    {
        KafkaProducerBaseFunctionality.SendMessage(_bootstrapServers, "remove-workers", email);
    }

    public void SendAddCustomerMessage(NameEmailDTO data)
    {
        KafkaProducerBaseFunctionality.SendMessage(_bootstrapServers, "add-customers", data);
    }

    public void SendAddCandidateMessage(CandidateMessageDTO data)
    {
        KafkaProducerBaseFunctionality.SendMessage(_bootstrapServers, "add-candidates", data);
    }
}
