using HiringService.Application.CQRS.HiringStageQueries;
using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Application.Exceptions.HiringStage;

namespace HiringService.Unit.Tests.CQRSTests.QueryTests.HiringStageTests;

public class GetHiringStageByIdTests
{

    private readonly Mock<IHiringStageRepository> _stageRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetHiringStageByIdHandler _handler;

    public GetHiringStageByIdTests()
    {
        _stageRepositoryMock = new Mock<IHiringStageRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetHiringStageByIdHandler(_stageRepositoryMock.Object, _mapperMock.Object, null!);
    }

    [Fact]
    public async Task Handle_ValidId_ReturnsHiringStageDTO()
    {
        // Arrange
        var id = 1;
        var hiringStage = new HiringStage { Id = id };
        var hiringStageDTO = new HiringStageMainInfoDTO { Id = id };

        _stageRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync(hiringStage);

        _mapperMock.Setup(mapper => mapper.Map<HiringStageMainInfoDTO>(hiringStage))
            .Returns(hiringStageDTO);

        var query = new GetHiringStageByIdQuery(id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(hiringStageDTO, result);
        _stageRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<HiringStageMainInfoDTO>(hiringStage), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidId_ThrowsNoHiringStageWithSuchIdException()
    {
        // Arrange
        var id = 1;

        var stageRepositoryMock = new Mock<IHiringStageRepository>();
        stageRepositoryMock.Setup(repo => repo.GetByIdAsync(id))
            .ReturnsAsync((HiringStage)null!);

        var query = new GetHiringStageByIdQuery(id);

        // Act & Assert
        await Assert.ThrowsAsync<NoHiringStageWithSuchIdException>(() => _handler.Handle(query, CancellationToken.None));
        _stageRepositoryMock.Verify(repo => repo.GetByIdAsync(id), Times.Once);
        _mapperMock.Verify(mapper => mapper.Map<HiringStageMainInfoDTO>(It.IsAny<HiringStage>()), Times.Never);
    }
}
