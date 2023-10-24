using HiringService.Application.CQRS.StageNameQueries;
using HiringService.Application.DTOs.StageNameDTOs;

namespace HiringService.Unit.Tests.CQRSTests.QueryTests.HiringStageNameTests;

public class GetHiringStageNamesTests
{
    private readonly Mock<IHiringStageNameRepository> _nameRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetHiringStageNamesHandler _handler;

    public GetHiringStageNamesTests()
    {
        _nameRepositoryMock = new Mock<IHiringStageNameRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetHiringStageNamesHandler(_nameRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsListOfStageNameDTOs()
    {
        // Arrange
        var stageNames = new List<HiringStageName>
        {
            new HiringStageName { Id = 1, Name = "Stage 1" },
            new HiringStageName { Id = 2, Name = "Stage 2" },
            new HiringStageName { Id = 3, Name = "Stage 3" }
        };

        var stageNameDTOs = new List<GetStageNameDTO>
        {
            new GetStageNameDTO { Id = 1, Name = "Stage 1" },
            new GetStageNameDTO { Id = 2, Name = "Stage 2" },
            new GetStageNameDTO { Id = 3, Name = "Stage 3" }
        };

        _nameRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(stageNames);
        _mapperMock.Setup(m => m.Map<GetStageNameDTO>(It.IsAny<HiringStageName>()))
            .Returns((HiringStageName stageName) => stageNameDTOs.FirstOrDefault(dto => dto.Id == stageName.Id));

        // Act
        var result = await _handler.Handle(new GetHiringStageNamesQuery(), CancellationToken.None);

        // Assert
        Assert.Equal(stageNameDTOs, result);
        _nameRepositoryMock.Verify(r => r.GetAllAsync(), Times.Once);
        _mapperMock.Verify(m => m.Map<GetStageNameDTO>(It.IsAny<HiringStageName>()), Times.Exactly(stageNames.Count));
    }
}
