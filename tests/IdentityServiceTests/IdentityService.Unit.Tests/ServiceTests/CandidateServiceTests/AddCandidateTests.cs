using IdentityService.Application.DTOs;
using IdentityService.Application.DTOs.CandidateDTO;
using IdentityService.Application.DTOs.CandidateDTOs;
using IdentityService.Application.Exceptions.Candidate;
using IdentityService.Application.KafkaAbstractions;

namespace IdentityService.Unit.Tests.ServiceTests.CandidateServiceTests;

public class AddCandidateTests
{
    private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
    private readonly Mock<IKafkaProducer> _kafkaProducerMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CandidateService _candidateService;

    public AddCandidateTests()
    {
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
        _kafkaProducerMock = new Mock<IKafkaProducer>();
        _mapperMock = new Mock<IMapper>();

        _candidateService = new CandidateService(
            _candidateRepositoryMock.Object,
            _mapperMock.Object,
            _kafkaProducerMock.Object
        );
    }

    [Fact]
    public async Task AddAsync_WithNewCandidate_ReturnsLogInData()
    {
        // Arrange
        var candidateDTO = new AddCandidateDTO();
        var newCandidate = new Candidate();
        var logInData = new LogInData();

        _candidateRepositoryMock
            .Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<Candidate, bool>>>()))
            .ReturnsAsync((Candidate)null!);
        _mapperMock.Setup(m => m.Map<Candidate>(candidateDTO))
            .Returns(newCandidate);
        _mapperMock.Setup(m => m.Map<LogInData>(newCandidate))
            .Returns(logInData);

        // Act
        var result = await _candidateService.AddAsync(candidateDTO);

        // Assert
        result.Should().Be(logInData);
        _candidateRepositoryMock.Verify(r => r.Add(newCandidate), Times.Once);
        _candidateRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        _kafkaProducerMock.Verify(p => p.SendAddCandidateMessage(It.IsAny<CandidateMessageDTO>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_WithExistingCandidate_ThrowsNoCandidateWithSuchEmailException()
    {
        // Arrange
        var candidateDTO = new AddCandidateDTO();
        var existingCandidate = new Candidate();

        _candidateRepositoryMock
            .Setup(r => r.GetFirstAsync(It.IsAny<Expression<Func<Candidate, bool>>>()))
            .ReturnsAsync(existingCandidate);

        // Act & Assert
        await Assert.ThrowsAsync<NoCandidateWithSuchEmailException>(
            () => _candidateService.AddAsync(candidateDTO)
        );

        _candidateRepositoryMock.Verify(r => r.Add(It.IsAny<Candidate>()), Times.Never);
        _candidateRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        _kafkaProducerMock.Verify(p => p.SendAddCandidateMessage(It.IsAny<CandidateMessageDTO>()), Times.Never);
    }
}
