using HiringService.Application.CQRS.CandidateCommands;
using HiringService.Application.DTOs.CandidateDTOs;
using HiringService.Application.Exceptions.Candidate;


namespace HiringService.Unit.Tests.CQRSTests.CommandTests.CandidateTests;

public class AddCandidateTests
{
    private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddCandidateHandler _handler;

    public AddCandidateTests()
    {
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new AddCandidateHandler(_candidateRepositoryMock.Object, _mapperMock.Object, null);
    }

    [Fact]
    public async Task Handle_WithNewCandidate_ReturnsCandidateId()
    {
        // Arrange
        var candidateDTO = new AddCandidateDTO { Email = "test@example.com", Name = "John Doe" };
        var candidate = new Candidate { Id = 1, Email = "test@example.com", Name = "John Doe" };

        _candidateRepositoryMock.Setup(r => r.GetByEmailAsync(candidateDTO.Email))
            .ReturnsAsync((Candidate)null);

        _mapperMock.Setup(m => m.Map<Candidate>(candidateDTO))
            .Returns(candidate);

        _candidateRepositoryMock.Setup(r => r.Add(candidate))
            .Returns(candidate);

        _candidateRepositoryMock.Setup(r => r.SaveChangesAsync());

        // Act
        var result = await _handler.Handle(new AddCandidateCommand(candidateDTO), CancellationToken.None);

        // Assert
        Assert.Equal(candidate.Id, result);
        _candidateRepositoryMock.Verify(r => r.GetByEmailAsync(candidateDTO.Email), Times.Once);
        _mapperMock.Verify(m => m.Map<Candidate>(candidateDTO), Times.Once);
        _candidateRepositoryMock.Verify(r => r.Add(candidate), Times.Once);
        _candidateRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WithExistingCandidate_ThrowsCandidateAlreadyExistsException()
    {
        // Arrange
        var candidateDTO = new AddCandidateDTO { Email = "test@example.com", Name = "John Doe" };
        var existingCandidate = new Candidate { Id = 1, Email = "test@example.com", Name = "John Doe" };

        _candidateRepositoryMock.Setup(r => r.GetByEmailAsync(candidateDTO.Email))
            .ReturnsAsync(existingCandidate);

        // Act & Assert
        await Assert.ThrowsAsync<CandidateAlreadyExistsException>(() =>
            _handler.Handle(new AddCandidateCommand(candidateDTO), CancellationToken.None));

        _candidateRepositoryMock.Verify(r => r.GetByEmailAsync(candidateDTO.Email), Times.Once);
        _mapperMock.Verify(m => m.Map<Candidate>(candidateDTO), Times.Never);
        _candidateRepositoryMock.Verify(r => r.Add(It.IsAny<Candidate>()), Times.Never);
        _candidateRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}
