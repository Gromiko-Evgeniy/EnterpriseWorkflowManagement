using ProjectManagementService.Application.Exceptions.ProjectTask;
using ProjectManagementService.Application.CQRS.ProjectTaskCommands;

namespace ProjectManagementService.Unit.Tests.CQRS.CommandTests.ProjectTaskTests;

public class FinishWorkingOnTaskTests
{
    private readonly Mock<IProjectTaskRepository> _taskRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly FinishWorkingOnTaskHandler _handler;

    public FinishWorkingOnTaskTests()
    {
        _taskRepositoryMock = new Mock<IProjectTaskRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new FinishWorkingOnTaskHandler(
            _taskRepositoryMock.Object,
            null!,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingWorkerTask_FinishesWorkingOnTask()
    {
        // Arrange
        var workerTask = new ProjectTask { Id = "1", WorkerId = "1"};
        var command = new FinishWorkingOnTaskCommand(workerTask.WorkerId);

        _taskRepositoryMock
            .Setup(r => r.GetByWorkerIdAsync(command.WorkerId))
            .ReturnsAsync(workerTask);

        var endTime = DateTime.Now;
        _taskRepositoryMock
            .Setup(r => r.FinishWorkingOnTask(workerTask.Id))
            .Callback(() => workerTask.FinishTime = endTime);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        workerTask.FinishTime.Should().Be(endTime);
        _taskRepositoryMock.Verify(r => r.FinishWorkingOnTask(workerTask.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingWorkerTask_ThrowsNoTaskWithSuchWorkerIdException()
    {
        // Arrange
        var command = new FinishWorkingOnTaskCommand("1");

        _taskRepositoryMock.Setup(r => r.GetByWorkerIdAsync(command.WorkerId))
            .ReturnsAsync((ProjectTask)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoTaskWithSuchWorkerIdException>(() => _handler.Handle(command, CancellationToken.None));
    }
}

