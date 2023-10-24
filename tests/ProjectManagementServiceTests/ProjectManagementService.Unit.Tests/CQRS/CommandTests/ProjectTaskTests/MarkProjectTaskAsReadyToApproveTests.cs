using ProjectManagementService.Application.Exceptions.ProjectTask;
using ProjectManagementService.Application.CQRS.ProjectTaskCommands;
using Moq;

namespace ProjectManagementService.Unit.Tests.CQRS.CommandTests.ProjectTaskTests;

public class MarkProjectTaskAsReadyToApproveTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly Mock<IProjectTaskRepository> _taskRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly MarkProjectTaskAsReadyToApproveHandler _handler;

    public MarkProjectTaskAsReadyToApproveTests()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _taskRepositoryMock = new Mock<IProjectTaskRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new MarkProjectTaskAsReadyToApproveHandler(
            _workerRepositoryMock.Object,
            _taskRepositoryMock.Object,
            _mapperMock.Object,
            null!
        );
    }

    [Fact]
    public async Task Handle_WithExistingWorkerTask_MarksTaskAsReadyToApprove()
    {
        // Arrange
        var worker = new Worker() { Id = "1" };
        var workerTask = new ProjectTask { Id = "1", WorkerId = worker.Id };
        var command = new MarkProjectTaskAsReadyToApproveCommand(workerTask.WorkerId);

        _taskRepositoryMock
            .Setup(r => r.GetByWorkerIdAsync(command.WorkerId))
            .ReturnsAsync(workerTask);

        _workerRepositoryMock
            .Setup(r => r.GetByIdAsync(command.WorkerId))
            .ReturnsAsync(worker);

        _taskRepositoryMock
            .Setup(r => r.MarkAsReadyToApproveAsync(workerTask.Id))
            .Callback(() => workerTask.Status = ProjectTaskStatus.ReadyToApprove);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        workerTask.Status.Should().Be(ProjectTaskStatus.ReadyToApprove);
        _taskRepositoryMock.Verify(r => r.MarkAsReadyToApproveAsync(workerTask.Id), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingWorkerTask_ThrowsNoTaskWithSuchWorkerIdException()
    {
        // Arrange
        var command = new MarkProjectTaskAsReadyToApproveCommand("1");

        _taskRepositoryMock
            .Setup(r => r.GetByWorkerIdAsync(command.WorkerId))
            .ReturnsAsync((ProjectTask)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoTaskWithSuchWorkerIdException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
