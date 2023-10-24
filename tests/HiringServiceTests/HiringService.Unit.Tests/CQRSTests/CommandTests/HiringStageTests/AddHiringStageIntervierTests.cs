using HiringService.Application.CQRS.HiringStageCommands;
using HiringService.Application.Exceptions.HiringStageName;
using HiringService.Application.Exceptions.Worker;

namespace HiringService.Unit.Tests.CQRSTests.CommandTests.HiringStageTests;

public class AddHiringStageIntervierTests
{
    private readonly Mock<IHiringStageRepository> _stageRepositoryMock;
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly AddHiringStageIntervierHandler _handler;

    public AddHiringStageIntervierTests()
    {
        _stageRepositoryMock = new Mock<IHiringStageRepository>();
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _handler = new AddHiringStageIntervierHandler(_stageRepositoryMock.Object, _workerRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingWorkerAndStage_AddsIntervierToStageAndSavesChanges()
    {
        // Arrange
        var command = new AddHiringStageIntervierCommand(1, 1);
        var worker = new Worker { Id = 1 };
        var stage = new HiringStage { Id = 1 };

        _workerRepositoryMock.Setup(r => r.GetByIdAsync(command.IntervierId))
            .ReturnsAsync(worker);
        _stageRepositoryMock.Setup(r => r.GetByIdAsync(command.StageId))
            .ReturnsAsync(stage);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(worker, stage.Interviewer);
        _stageRepositoryMock.Verify(r => r.Update(stage), Times.Once);
        _stageRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingWorker_ThrowsNoWorkerWithSuchIdException()
    {
        // Arrange
        var command = new AddHiringStageIntervierCommand(1, 1);

        _workerRepositoryMock.Setup(r => r.GetByIdAsync(command.IntervierId))
            .ReturnsAsync((Worker)null);

        // Act & Assert
        await Assert.ThrowsAsync<NoWorkerWithSuchIdException>(() => _handler.Handle(command, CancellationToken.None));

        // Assert
        _stageRepositoryMock.Verify(r => r.Update(It.IsAny<HiringStage>()), Times.Never);
        _stageRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_WithNonExistingStage_ThrowsNoStageNameWithSuchIdException()
    {
        // Arrange
        var command = new AddHiringStageIntervierCommand(1, 1);
        var worker = new Worker { Id = 1 };

        _workerRepositoryMock.Setup(r => r.GetByIdAsync(command.IntervierId))
            .ReturnsAsync(worker);
        _stageRepositoryMock.Setup(r => r.GetByIdAsync(command.StageId))
            .ReturnsAsync((HiringStage)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoStageNameWithSuchIdException>(() => _handler.Handle(command, CancellationToken.None));

        // Assert
        _stageRepositoryMock.Verify(r => r.Update(It.IsAny<HiringStage>()), Times.Never);
        _stageRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
    }
}
