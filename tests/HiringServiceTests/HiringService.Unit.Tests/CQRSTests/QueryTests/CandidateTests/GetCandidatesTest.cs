using HiringService.Application.CQRS.CandidateQueries;
using HiringService.Application.DTOs.CandidateDTOs;

namespace HiringService.Unit.Tests.CQRSTests.QueryTests.CandidateTests;

public class GetCandidatesTest
{
    private Mock<ICandidateRepository> _candidateRepositoryMock;
    private Mock<IMapper> _mapperMock;
    private GetCandidatesHandler _handler;

    public GetCandidatesTest()
    {
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetCandidatesHandler(_candidateRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsListOfCandidateDtos()
    {
        // Arrange
        var candidates = new List<Candidate>();
        _candidateRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(candidates);

        _mapperMock.Setup(mapper => mapper.Map<CandidateShortInfoDTO>(It.IsAny<Candidate>()))
            .Returns(new CandidateShortInfoDTO());

        var candidateDtos = new List<CandidateShortInfoDTO>();

        var query = new GetCandidatesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(candidateDtos, result);
    }

    [Fact]
    public async Task Handle_CallsCandidateRepositoryGetAllAsync()
    {
        // Arrange
        var candidates = new List<Candidate>();
        _candidateRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(candidates);

        var candidateDtos = new List<CandidateShortInfoDTO>();
        _mapperMock.Setup(mapper => mapper.Map<CandidateShortInfoDTO>(It.IsAny<Candidate>()))
            .Returns(new CandidateShortInfoDTO());

        var query = new GetCandidatesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _candidateRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_CallsMapperForEachCandidate()
    {
        // Arrange
        var candidates = new List<Candidate> { new Candidate(), new Candidate() };
        _candidateRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(candidates);

        var candidateDtos = new List<CandidateShortInfoDTO>();
        _mapperMock.Setup(mapper => mapper.Map<CandidateShortInfoDTO>(It.IsAny<Candidate>()))
            .Returns(new CandidateShortInfoDTO());

        var query = new GetCandidatesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        _mapperMock.Verify(
            mapper => mapper.Map<CandidateShortInfoDTO>(It.IsAny<Candidate>()),
            Times.Exactly(candidates.Count));
    }
}
