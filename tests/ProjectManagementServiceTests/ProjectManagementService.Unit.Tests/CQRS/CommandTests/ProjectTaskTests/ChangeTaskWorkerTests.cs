using ProjectManagementService.Application.Exceptions.ProjectTask;
using ProjectManagementService.Application.CQRS.ProjectTaskCommands;
using ProjectManagementService.Application.Exceptions.Worker;

namespace ProjectManagementService.Unit.Tests.CQRS.CommandTests.ProjectTaskTests;

public class ChangeTaskWorkerTests
{
    private readonly Mock<IProjectTaskRepository> _taskRepositoryMock;
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ChangeTaskWorkerHandler _handler;

    public ChangeTaskWorkerTests()
    {
        _taskRepositoryMock = new Mock<IProjectTaskRepository>();
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new ChangeTaskWorkerHandler(
            _taskRepositoryMock.Object,
            _workerRepositoryMock.Object,
            null!, //cache
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingTaskAndWorker_UpdatesTaskWorkerId()
    {
        // Arrange
        var task = new ProjectTask { Id = "1" };
        var worker = new Worker { Id = "2" };
        var command = new ChangeTaskWorkerCommand(worker.Id, task.Id);

        _taskRepositoryMock
            .Setup(r => r.GetByIdAsync(command.TaskId))
            .ReturnsAsync(task);

        _workerRepositoryMock
            .Setup(r => r.GetByIdAsync(command.WorkerId))
            .ReturnsAsync(worker);

        _taskRepositoryMock
            .Setup(r => r.UpdateWorkerIdAsync(command.TaskId, command.WorkerId))
            .Callback(() => task.WorkerId = command.WorkerId);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        task.WorkerId.Should().Be(command.WorkerId);
        _taskRepositoryMock.Verify(r => r.UpdateWorkerIdAsync(task.Id, command.WorkerId), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingTask_ThrowsNoProjectTaskWithSuchIdException()
    {
        // Arrange
        var command = new ChangeTaskWorkerCommand("1", "2");

        _taskRepositoryMock
            .Setup(r => r.GetByIdAsync(command.TaskId))
            .ReturnsAsync((ProjectTask)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoProjectTaskWithSuchIdException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithNonExistingWorker_ThrowsNoWorkerWithSuchIdException()
    {
        // Arrange
        var command = new ChangeTaskWorkerCommand("1", "2");

        var task = new ProjectTask { Id = command.TaskId };

        _taskRepositoryMock.Setup(r => r.GetByIdAsync(command.TaskId))
            .ReturnsAsync(task);

        _workerRepositoryMock.Setup(r => r.GetByIdAsync(command.WorkerId))
            .ReturnsAsync((Worker)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoWorkerWithSuchIdException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
