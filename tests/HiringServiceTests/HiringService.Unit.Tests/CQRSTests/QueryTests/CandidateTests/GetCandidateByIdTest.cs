using HiringService.Application.CQRS.CandidateQueries;
using HiringService.Application.DTOs.CandidateDTOs;
using HiringService.Application.Exceptions.Candidate;

namespace HiringService.Unit.Tests.CQRSTests.QueryTests.CandidateTests;

public class GetCandidateByIdTest
{
    private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetCandidateByIdHandler _handler;

    public GetCandidateByIdTest()
    {
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetCandidateByIdHandler(_candidateRepositoryMock.Object, _mapperMock.Object, null);
    }

    [Fact]
    public async Task Handle_ReturnsCandidateDTO_WhenCandidateExists()
    {
        // Arrange
        var query = new GetCandidateByIdQuery(1);
        var candidate = new Candidate();
        var candidateDTO = new CandidateMainInfoDTO();
        _candidateRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(candidate);
        _mapperMock.Setup(mapper => mapper.Map<CandidateMainInfoDTO>(candidate)).Returns(candidateDTO);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(candidateDTO, result);
        _candidateRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<CandidateMainInfoDTO>(candidate), Times.Once);
    }

    [Fact]
    public async Task Handle_ThrowsNoCandidateWithSuchIdException_WhenCandidateDoesNotExist()
    {
        // Arrange
        var query = new GetCandidateByIdQuery(1);
        _candidateRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Candidate)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoCandidateWithSuchIdException>(() => _handler.Handle(query, CancellationToken.None));
        _candidateRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<CandidateMainInfoDTO>(It.IsAny<Candidate>()), Times.Never);
    }
}
