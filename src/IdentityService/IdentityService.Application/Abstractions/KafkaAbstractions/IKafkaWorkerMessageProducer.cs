using IdentityService.Application.DTOs;
using IdentityService.Application.DTOs.CandidateDTOs;

namespace IdentityService.Application.KafkaAbstractions;

public interface IKafkaProducer
{
    public void SendAddWorkerMessage(NameEmailDTO data);

    public void SendRemoveWorkerMessage(string email);

    public void SendAddCustomerMessage(NameEmailDTO data);

    public void SendAddCandidateMessage(CandidateMessageDTO data);
}
