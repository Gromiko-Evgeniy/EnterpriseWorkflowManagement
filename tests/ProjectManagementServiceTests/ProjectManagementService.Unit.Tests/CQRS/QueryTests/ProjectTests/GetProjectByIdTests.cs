using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.ProjectDTOs;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Application.CQRS.ProjectQueries;

namespace ProjectManagementService.Unit.Tests.CQRS.QueryTests.ProjectTests;

public class GetProjectByIdTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetProjectByIdHandler _handler;

    public GetProjectByIdTests()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetProjectByIdHandler(
            _projectRepositoryMock.Object,
            null!,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithExistingProjectId_ReturnsProjectMainInfoDTO()
    {
        // Arrange
        var project = new Project { Id = "1" };
        var projectMainInfoDTO = new ProjectMainInfoDTO();
        var query = new GetProjectByIdQuery(project.Id);

        _projectRepositoryMock.Setup(r => r.GetByIdAsync(query.Id))
            .ReturnsAsync(project);
        _mapperMock.Setup(m => m.Map<ProjectMainInfoDTO>(project))
            .Returns(projectMainInfoDTO);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(projectMainInfoDTO);
    }

    [Fact]
    public async Task Handle_WithNonExistingProjectId_ThrowsNoProjectWithSuchIdException()
    {
        // Arrange
        var query = new GetProjectByIdQuery("1");
        _projectRepositoryMock.Setup(r => r.GetByIdAsync(query.Id))
            .ReturnsAsync((Project)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoProjectWithSuchIdException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
