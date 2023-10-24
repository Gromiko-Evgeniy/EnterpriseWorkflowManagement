using ProjectManagementService.Application.Exceptions.ProjectTask;
using ProjectManagementService.Application.CQRS.ProjectTaskCommands;

namespace ProjectManagementService.Unit.Tests.CQRS.CommandTests.ProjectTaskTests;

public class StartWorkingOnTaskTests
{
    private readonly Mock<IProjectTaskRepository> _taskRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly StartWorkingOnTaskHandler _handler;

    public StartWorkingOnTaskTests()
    {
        _taskRepositoryMock = new Mock<IProjectTaskRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new StartWorkingOnTaskHandler(
            _taskRepositoryMock.Object,
            null!,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingWorkerTask_StartesWorkingOnTask()
    {
        // Arrange
        var workerTask = new ProjectTask { Id = "1", WorkerId = "1" };
        var command = new StartWorkingOnTaskCommand(workerTask.WorkerId);

        _taskRepositoryMock
            .Setup(r => r.GetByWorkerIdAsync(command.WorkerId))
            .ReturnsAsync(workerTask);

        var startTime = DateTime.Now;
        _taskRepositoryMock
            .Setup(r => r.StartWorkingOnTask(workerTask.Id))
            .Callback(() => workerTask.StartTime = startTime);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        workerTask.StartTime.Should().Be(startTime);
        _taskRepositoryMock.Verify(r => r.StartWorkingOnTask(workerTask.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingWorkerTask_ThrowsNoTaskWithSuchWorkerIdException()
    {
        // Arrange
        var command = new StartWorkingOnTaskCommand("1");

        _taskRepositoryMock.Setup(r => r.GetByWorkerIdAsync(command.WorkerId))
            .ReturnsAsync((ProjectTask)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoTaskWithSuchWorkerIdException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
