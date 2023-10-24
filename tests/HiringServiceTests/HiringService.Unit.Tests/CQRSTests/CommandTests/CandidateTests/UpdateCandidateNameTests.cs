using HiringService.Application.CQRS.CandidateCommands;
using HiringService.Application.DTOs.CandidateDTOs;
using HiringService.Application.Exceptions.Candidate;

namespace HiringService.Unit.Tests.CQRSTests.CommandTests.CandidateTests;

public class UpdateCandidateNameTests
{
    private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly UpdateCandidateNameHandler _handler;

    public UpdateCandidateNameTests()
    {
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new UpdateCandidateNameHandler(_candidateRepositoryMock.Object, _mapperMock.Object, null);
    }

    [Fact]
    public async Task Handle_ValidId_UpdatesCandidateName()
    {
        // Arrange
        var id = 1;
        var name = "John Doe";
        var candidate = new Candidate { Id = id, Name = "Jane Smith" };

        _candidateRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(candidate);

        candidate.Name = name;
        _candidateRepositoryMock.Setup(repo => repo.Update(candidate));

        // Act
        await _handler.Handle(new UpdateCandidateNameCommand(id, name), CancellationToken.None);

        // Assert
        _candidateRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        _candidateRepositoryMock.Verify(repo => repo.Update(candidate), Times.Once);
        _candidateRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<CandidateMainInfoDTO>(It.IsAny<Candidate>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidId_ThrowsNoCandidateWithSuchIdException()
    {
        // Arrange
        var id = 1;
        var name = "John Doe";

        _candidateRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync((Candidate)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoCandidateWithSuchIdException>(() => _handler.Handle(new UpdateCandidateNameCommand(id, name), CancellationToken.None));
        _candidateRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        _candidateRepositoryMock.Verify(repo => repo.Update(It.IsAny<Candidate>()), Times.Never);
        _candidateRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        _mapperMock.Verify(mapper => mapper.Map<CandidateMainInfoDTO>(It.IsAny<Candidate>()), Times.Never);
    }
}

