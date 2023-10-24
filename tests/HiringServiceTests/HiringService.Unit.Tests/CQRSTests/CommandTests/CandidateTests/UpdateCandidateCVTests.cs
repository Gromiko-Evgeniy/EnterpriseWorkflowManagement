using HiringService.Application.CQRS.CandidateCommands;
using HiringService.Application.Exceptions.Candidate;

namespace HiringService.Unit.Tests.CQRSTests.CommandTests.CandidateTests;

public class UpdateCandidateCVTests
{
    private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly UpdateCandidateCVHandler _handler;

    public UpdateCandidateCVTests()
    {
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
        _mapperMock = new Mock<IMapper>();
        _cacheMock = new Mock<IDistributedCache>();
        _handler = new UpdateCandidateCVHandler(_candidateRepositoryMock.Object, _mapperMock.Object, _cacheMock.Object);
    }

    [Fact]
    public async Task Handle_ValidId_UpdatesCandidateCV()
    {
        // Arrange
        var id = 1;
        var cv = "New CV";
        var candidate = new Candidate { Id = id, CV = "Old CV" };

        _candidateRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(candidate);

        candidate.CV = cv;
        _candidateRepositoryMock.Setup(repo => repo.Update(candidate));

        // Act
        await _handler.Handle(new UpdateCandidateCVCommand(id, cv), CancellationToken.None);

        // Assert
        _candidateRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        _candidateRepositoryMock.Verify(repo => repo.Update(candidate), Times.Once);
        _candidateRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidId_ThrowsNoCandidateWithSuchIdException()
    {
        // Arrange
        var id = 1;
        var cv = "New CV";

        _candidateRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync((Candidate)null);

        // Act & Assert
        await Assert.ThrowsAsync<NoCandidateWithSuchIdException>(() => _handler.Handle(new UpdateCandidateCVCommand(id, cv), CancellationToken.None));
        _candidateRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        _candidateRepositoryMock.Verify(repo => repo.Update(It.IsAny<Candidate>()), Times.Never);
        _candidateRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }
}
