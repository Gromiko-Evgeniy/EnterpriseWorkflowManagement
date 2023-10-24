using HiringService.Application.CQRS.HiringStageQueries;
using HiringService.Application.DTOs.HiringStageDTOs;

namespace HiringService.Unit.Tests.CQRSTests.QueryTests.HiringStageTests;

public class GetHiringStagesTests
{
    private readonly Mock<IHiringStageRepository> _stageRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetHiringStagesHandler _handler;

    public GetHiringStagesTests()
    {
        _stageRepositoryMock = new Mock<IHiringStageRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetHiringStagesHandler(_stageRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsListOfStageDTOs_WhenStagesExist()
    {
        // Arrange
        var query = new GetHiringStagesQuery();
        var stages = new List<HiringStage>();
        var stageDTOs = new List<HiringStageShortInfoDTO>();
        _stageRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(stages);
        _mapperMock
            .Setup(mapper => mapper.Map<HiringStageShortInfoDTO>(It.IsAny<HiringStage>()))
            .Returns((HiringStage stage) =>
                new HiringStageShortInfoDTO
                {
                    Description = stage.Description,
                    PassedSuccessfully = stage.PassedSuccessfully,
                    DateTime = stage.DateTime
                });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(stageDTOs, result);

        _stageRepositoryMock.Verify(
            repo => repo.GetAllAsync(),
            Times.Once);

        _mapperMock.Verify(
            mapper => mapper.Map<HiringStageShortInfoDTO>(It.IsAny<HiringStage>()),
            Times.Exactly(stages.Count));
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoStagesExist()
    {
        // Arrange
        var query = new GetHiringStagesQuery();
        var stages = new List<HiringStage>();
        _stageRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(stages);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
        _stageRepositoryMock.Verify(
            repo => repo.GetAllAsync(),
            Times.Once);

        _mapperMock.Verify(
            mapper => mapper.Map<HiringStageShortInfoDTO>(It.IsAny<HiringStage>()),
            Times.Never);
    }
}
