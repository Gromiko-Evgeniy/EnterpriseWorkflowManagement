using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Application.Exceptions.ProjectTask;
using ProjectManagementService.Application.CQRS.ProjectTaskCommands;

namespace ProjectManagementService.Unit.Tests.CQRS.CommandTests.ProjectTaskTests;

public class MarkProjectTaskAsApprovedTests
{
    private readonly Mock<IProjectTaskRepository> _taskRepositoryMock;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly MarkProjectTaskAsApprovedHandler _handler; 

    public MarkProjectTaskAsApprovedTests()
    {
        _taskRepositoryMock = new Mock<IProjectTaskRepository>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _workerRepositoryMock = new Mock<IWorkerRepository>();

        _handler = new MarkProjectTaskAsApprovedHandler(
            _taskRepositoryMock.Object,
            _projectRepositoryMock.Object,
            _workerRepositoryMock.Object,
            null!,
            new Mock<IMapper>().Object);
    }

    [Fact]
    public async Task Handle_WithExistingTaskAndProjectLeader_UpdatesTaskStatusAndSetsNewTaskForWorker()
    {
        // Arrange
        var existingProject = new Project { Id = "1" };
        var existingTask = new ProjectTask()
        {
            Id = "1",
            ProjectId = existingProject.Id, 
            WorkerId = "1" 
        };
        var command = new MarkProjectTaskAsApprovedCommand(existingTask.Id, existingTask.WorkerId);

        _taskRepositoryMock.Setup(r => r.GetByIdAsync(command.ProjectTaskId))
            .ReturnsAsync(existingTask);

        _projectRepositoryMock.Setup(r => r.GetProjectByProjectLeaderId(command.ProjectLeaderId))
            .ReturnsAsync(existingProject);

        _taskRepositoryMock.Setup(r => r.MarkAsApproved(command.ProjectTaskId))
            .Returns(Task.CompletedTask);

        _workerRepositoryMock.Setup(r => r.GetByIdAsync(existingTask.WorkerId))
            .ReturnsAsync(new Worker { Id = existingTask.WorkerId });

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        existingTask.Status.Should().Be(ProjectTaskStatus.Approved);
        _taskRepositoryMock.Verify(r => r.SetNewWorker(existingTask.Id, existingTask.WorkerId), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingTask_ThrowsNoProjectTaskWithSuchIdException()
    {
        // Arrange
        var command = new MarkProjectTaskAsApprovedCommand("1", "1");

        _taskRepositoryMock.Setup(r => r.GetByIdAsync(command.ProjectTaskId))
            .ReturnsAsync((ProjectTask)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoProjectTaskWithSuchIdException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithNonExistingProject_ThrowsNoProjectWithSuchIdException()
    {
        // Arrange
        var existingTask = new ProjectTask { Id = "1", ProjectId = "1" };
        var command = new MarkProjectTaskAsApprovedCommand(existingTask.Id, "1");

        _taskRepositoryMock.Setup(r => r.GetByIdAsync(command.ProjectTaskId))
            .ReturnsAsync(existingTask);
        _projectRepositoryMock.Setup(r => r.GetProjectByProjectLeaderId(command.ProjectLeaderId))
            .ReturnsAsync((Project)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoProjectWithSuchIdException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithTaskNotBelongingToProject_ThrowsAccessToApproveProjectTaskDeniedException()
    {
        // Arrange
        var existingTask = new ProjectTask { Id = "1", ProjectId = "2" };
        var projectLeaderProject = new Project { Id = "1" };
        var command = new MarkProjectTaskAsApprovedCommand("1", "1");

        _taskRepositoryMock.Setup(r => r.GetByIdAsync(command.ProjectTaskId))
            .ReturnsAsync(existingTask);

        _projectRepositoryMock.Setup(r => r.GetProjectByProjectLeaderId(command.ProjectLeaderId))
            .ReturnsAsync(projectLeaderProject);

        // Act & Assert
        await Assert.ThrowsAsync<AccessToApproveProjectTaskDeniedException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
