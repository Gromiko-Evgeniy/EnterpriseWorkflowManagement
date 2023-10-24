using ProjectManagementService.Application.ProjectDTOs;
using ProjectManagementService.Application.Abstractions.RepositoryAbstractions;
using ProjectManagementService.Application.CQRS.ProjectQueries;

namespace ProjectManagementService.Unit.Tests.CQRS.QueryTests.ProjectTests;

public class GetAllProjectsTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetAllProjectsHandler _handler;

    public GetAllProjectsTests()
    {
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetAllProjectsHandler(
            _projectRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ReturnsListOfProjectShortInfoDTO()
    {
        // Arrange
        var query = new GetAllProjectsQuery();
        var projects = new List<Project>
        {
            new Project { Id = "1" },
            new Project { Id = "2" } 
        };
        var projectShortInfoDTOs = new List<ProjectShortInfoDTO> 
        { 
            new ProjectShortInfoDTO { Id = "1" },
            new ProjectShortInfoDTO { Id = "2" } 
        };

        _projectRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(projects);
        _mapperMock.Setup(m => m.Map<ProjectShortInfoDTO>(It.IsAny<Project>()))
            .Returns<Project>(p => new ProjectShortInfoDTO { Id = p.Id });

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.Should().BeEquivalentTo(projectShortInfoDTOs);
    }
}
