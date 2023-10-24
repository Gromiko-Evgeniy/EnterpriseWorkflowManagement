using HiringService.Application.CQRS.HiringStageQueries;
using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Application.Exceptions.Worker;

namespace HiringService.Unit.Tests.CQRSTests.QueryTests.HiringStageTests;

public class GetHiringStagesByIntervierIdTests
{
    private readonly Mock<IHiringStageRepository> _stageRepositoryMock;
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetHiringStagesByIntervierIdHandler _handler;

    public GetHiringStagesByIntervierIdTests()
    {
        _stageRepositoryMock = new Mock<IHiringStageRepository>();
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetHiringStagesByIntervierIdHandler(_stageRepositoryMock.Object, _workerRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ThrowsNoWorkerWithSuchIdException_WhenWorkerDoesNotExist()
    {
        // Arrange
        var query = new GetHiringStagesByIntervierIdQuery(1);
        _workerRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Worker)null);

        // Act & Assert
        await Assert.ThrowsAsync<NoWorkerWithSuchIdException>(() => _handler.Handle(query, CancellationToken.None));
        _workerRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        _stageRepositoryMock.Verify(repo => repo.GetByIntervierIdAsync(It.IsAny<int>()), Times.Never);
        _mapperMock.Verify(mapper => mapper.Map<HiringStageShortInfoDTO>(It.IsAny<HiringStage>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ReturnsListOfStageDTOs_WhenWorkerExists()
    {
        // Arrange
        var query = new GetHiringStagesByIntervierIdQuery(1);
        var worker = new Worker { Id = 1 };
        var stages = new List<HiringStage>();
        var stageDTOs = new List<HiringStageShortInfoDTO>();
        _workerRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(worker);
        _stageRepositoryMock.Setup(repo => repo.GetByIntervierIdAsync(1)).ReturnsAsync(stages);
        _mapperMock
            .Setup(mapper => mapper.Map<HiringStageShortInfoDTO>(It.IsAny<HiringStage>()))
            .Returns((HiringStage stage) => 
                new HiringStageShortInfoDTO { 
                    Description = stage.Description, 
                    PassedSuccessfully = stage.PassedSuccessfully, 
                    DateTime = stage.DateTime 
                });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(stageDTOs, result);

        _workerRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);

        _stageRepositoryMock.Verify(repo => repo.GetByIntervierIdAsync(1), Times.Once);

        _mapperMock.Verify(
            mapper => mapper.Map<HiringStageShortInfoDTO>(It.IsAny<HiringStage>()),
            Times.Exactly(stages.Count));
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoStagesExistForWorker()
    {
        // Arrange
        var query = new GetHiringStagesByIntervierIdQuery(1);
        var worker = new Worker { Id = 1 };
        var stages = new List<HiringStage>();
        _workerRepositoryMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(worker);
        _stageRepositoryMock.Setup(repo => repo.GetByIntervierIdAsync(1)).ReturnsAsync(stages);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
        _workerRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        _stageRepositoryMock.Verify(repo => repo.GetByIntervierIdAsync(1), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<HiringStageShortInfoDTO>(It.IsAny<HiringStage>()), Times.Never);
    }
}
