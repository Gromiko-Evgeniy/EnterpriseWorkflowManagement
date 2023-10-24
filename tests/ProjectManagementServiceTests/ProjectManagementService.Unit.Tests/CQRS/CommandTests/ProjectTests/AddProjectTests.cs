using ProjectManagementService.Application.CQRS.ProjectCommands;
using ProjectManagementService.Application.ProjectDTOs;

namespace ProjectManagementService.Unit.Tests.CQRS.CommandTests.ProjectTests;

public class AddProjectTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddProjectHandler _handler;

    public AddProjectTests()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new AddProjectHandler(
            _projectRepositoryMock.Object,
            null!,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidRequest_ReturnsProjectId()
    {
        // Arrange
        var command = new AddProjectCommand(new AddProjectDTO(), "1");
        var project = new Project { Id = "1" };

        _mapperMock.Setup(m => m.Map<Project>(command.ProjectDTO))
            .Returns(project);
        _projectRepositoryMock.Setup(r => r.AddOneAsync(project))
            .ReturnsAsync(project.Id);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(project.Id);
    }
}
