using ProjectManagementService.Application.Exceptions.ProjectTask;
using ProjectManagementService.Domain.Enumerations;
using ProjectManagementService.Application.CQRS.ProjectTaskCommands;

namespace ProjectManagementService.Unit.Tests.CQRS.CommandTests.ProjectTaskTests;

public class CancelProjectTaskByIdTests
{
    private readonly Mock<IProjectTaskRepository> _taskRepositoryMock;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CancelProjectTaskByIdHandler _handler;

    public CancelProjectTaskByIdTests()
    {
        _taskRepositoryMock = new Mock<IProjectTaskRepository>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new CancelProjectTaskByIdHandler(
            _taskRepositoryMock.Object,
            _projectRepositoryMock.Object,
            null!,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithExistingTaskIdAndMatchingCustomerId_CancelsTask()
    {
        // Arrange
        var project = new Project { Id = "1" };
        var customerProjects = new List<Project> { project };
        var task = new ProjectTask { Id = "1", ProjectId = project.Id, Status = ProjectTaskStatus.ToDo };
        var command = new CancelProjectTaskByIdCommand(task.Id, "1");

        _taskRepositoryMock.Setup(r => r.GetByIdAsync(command.ProjectTaskId))
            .ReturnsAsync(task);
        _projectRepositoryMock.Setup(r => r.GetAllCustomerProjectsAsync(command.CustomerId))
            .ReturnsAsync(customerProjects);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _taskRepositoryMock.Verify(r => r.CancelAsync(command.ProjectTaskId), Times.Once);
        task.Status.Should().Be(ProjectTaskStatus.Canceled);
    }

    [Fact]
    public async Task Handle_WithNonExistingTaskId_ThrowsNoProjectTaskWithSuchIdException()
    {
        // Arrange
        var command = new CancelProjectTaskByIdCommand("1", "1");

        _taskRepositoryMock.Setup(r => r.GetByIdAsync(command.ProjectTaskId))
            .ReturnsAsync((ProjectTask)null!);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NoProjectTaskWithSuchIdException>();
    }

    [Fact]
    public async Task Handle_WithExistingTaskIdAndNonMatchingCustomerId_ThrowsAccessToCancelProjectTaskDeniedException()
    {
        // Arrange
        var task = new ProjectTask { Id = "1", ProjectId = "1" };
        var command = new CancelProjectTaskByIdCommand(task.Id, "2");

        _taskRepositoryMock.Setup(r => r.GetByIdAsync(command.ProjectTaskId))
            .ReturnsAsync(task);
        _projectRepositoryMock.Setup(r => r.GetAllCustomerProjectsAsync(command.CustomerId))
            .ReturnsAsync(new List<Project>());

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<AccessToCancelProjectTaskDeniedException>();
    }
}

