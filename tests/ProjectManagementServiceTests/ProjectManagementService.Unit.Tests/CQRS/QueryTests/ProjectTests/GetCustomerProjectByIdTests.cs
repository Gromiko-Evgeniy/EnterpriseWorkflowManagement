using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.ProjectDTOs;
using ProjectManagementService.Application.Exceptions.Project;
using ProjectManagementService.Application.CQRS.ProjectQueries;

namespace ProjectManagementService.Unit.Tests.CQRS.QueryTests.ProjectTests;

public class GetCustomerProjectByIdTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetCustomerProjectByIdHandler _handler;

    public GetCustomerProjectByIdTests()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetCustomerProjectByIdHandler(
            _projectRepositoryMock.Object,
            null!,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithExistingProjectIdAndMatchingCustomerId_ReturnsProjectMainInfoDTO()
    {
        // Arrange
        var project = new Project { Id = "1", CustomerId = "1" };
        var projectMainInfoDTO = new ProjectMainInfoDTO();
        var query = new GetCustomerProjectByIdQuery(project.Id, project.CustomerId);

        _projectRepositoryMock
            .Setup(r => r.GetByIdAsync(query.ProjectId))
            .ReturnsAsync(project);

        _mapperMock
            .Setup(m => m.Map<ProjectMainInfoDTO>(project))
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
        var query = new GetCustomerProjectByIdQuery("1", "1");
        _projectRepositoryMock
            .Setup(r => r.GetByIdAsync(query.ProjectId))
            .ReturnsAsync((Project)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoProjectWithSuchIdException>(() => _handler.Handle(query, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithExistingProjectIdAndNonMatchingCustomerId_ThrowsCustomerAccessToProjectDeniedException()
    {
        // Arrange
        var query = new GetCustomerProjectByIdQuery("1", "1");
        var project = new Project { Id = "1", CustomerId = "2" };

        _projectRepositoryMock.Setup(r => r.GetByIdAsync(query.ProjectId))
            .ReturnsAsync(project);

        // Act & Assert
        await Assert.ThrowsAsync<CustomerAccessToProjectDeniedException>(() => _handler.Handle(query, CancellationToken.None));
    }
}
