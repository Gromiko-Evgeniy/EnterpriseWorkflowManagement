using ProjectManagementService.Application.CQRS.ProjectTaskCommands;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Application.TaskDTOs;

namespace ProjectManagementService.Unit.Tests.CQRS.CommandTests.ProjectTaskTests;

public class AddProjectTaskTests
{
    private readonly Mock<IProjectTaskRepository> _projectTaskRepositoryMock;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddProjectTaskHandler _handler;

    public AddProjectTaskTests()
    {
        _projectTaskRepositoryMock = new Mock<IProjectTaskRepository>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new AddProjectTaskHandler(
            _projectTaskRepositoryMock.Object,
            _projectRepositoryMock.Object,
            null!,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReturnsTaskId()
    {
        // Arrange
        var project = new Project { Id = "1" };
        var command = new AddProjectTaskCommand(new AddProjectTaskDTO() { ProjectId = project.Id });
        var projectTask = new ProjectTask { Id = "1" };

        _projectRepositoryMock
            .Setup(r => r.GetByIdAsync(command.ProjectTaskDTO.ProjectId))
            .ReturnsAsync(project);

        _mapperMock
            .Setup(m => m.Map<ProjectTask>(command.ProjectTaskDTO))
            .Returns(projectTask);

        _projectTaskRepositoryMock
            .Setup(r => r.AddOneAsync(projectTask))
            .ReturnsAsync(projectTask.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(projectTask.Id);
    }

    [Fact]
    public async Task Handle_WithNonExistingProject_ThrowsNoProjectWithSuchIdException()
    {
        // Arrange
        var command = new AddProjectTaskCommand(new AddProjectTaskDTO() { ProjectId = "1" });

        _projectRepositoryMock
            .Setup(r => r.GetByIdAsync(command.ProjectTaskDTO.ProjectId))
            .ReturnsAsync((Project)null!);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NoProjectWithSuchIdException>();
    }
}
