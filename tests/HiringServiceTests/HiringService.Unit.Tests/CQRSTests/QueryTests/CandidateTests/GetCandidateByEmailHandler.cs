using HiringService.Application.CQRS.CandidateQueries;
using HiringService.Application.DTOs.CandidateDTOs;
using HiringService.Application.Exceptions.Candidate;

namespace HiringService.Unit.Tests.CQRSTests.QueryTests.CandidateTests;

public class GetCandidateByEmailTests
{
    private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetCandidateByEmailHandler _handler;

    public GetCandidateByEmailTests()
    {
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetCandidateByEmailHandler(_candidateRepositoryMock.Object, _mapperMock.Object, null!);
    }

    [Fact]
    public async Task Handle_ReturnsCandidateDTO_WhenCandidateExists()
    {
        // Arrange
        var query = new GetCandidateByEmailQuery("test@example.com");
        var candidate = new Candidate();
        var candidateDTO = new CandidateMainInfoDTO();
        _candidateRepositoryMock.Setup(repo => repo.GetByEmailAsync("test@example.com")).ReturnsAsync(candidate);
        _mapperMock.Setup(mapper => mapper.Map<CandidateMainInfoDTO>(candidate)).Returns(candidateDTO);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(candidateDTO, result);
        _candidateRepositoryMock.Verify(repo => repo.GetByEmailAsync("test@example.com"), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<CandidateMainInfoDTO>(candidate), Times.Once);
    }

    [Fact]
    public async Task Handle_ThrowsNoCandidateWithSuchEmailException_WhenCandidateDoesNotExist()
    {
        // Arrange
        var query = new GetCandidateByEmailQuery("test@example.com");
        _candidateRepositoryMock.Setup(repo => repo.GetByEmailAsync("test@example.com")).ReturnsAsync((Candidate)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoCandidateWithSuchEmailException>(() => _handler.Handle(query, CancellationToken.None));
        _candidateRepositoryMock.Verify(repo => repo.GetByEmailAsync("test@example.com"), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<CandidateMainInfoDTO>(It.IsAny<Candidate>()), Times.Never);
    }
}
