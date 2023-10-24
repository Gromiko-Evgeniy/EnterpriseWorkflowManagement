using HiringService.Application.CQRS.StageNameQueries;
using HiringService.Application.DTOs.StageNameDTOs;
using HiringService.Application.Exceptions.HiringStageName;

namespace HiringService.Unit.Tests.CQRSTests.QueryTests.HiringStageNameTests;

public class GetHiringStageNameByIdTests
{
    private readonly Mock<IHiringStageNameRepository> _nameRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly GetHiringStageNameByIdHandler _handler;

    public GetHiringStageNameByIdTests()
    {
        _nameRepositoryMock = new Mock<IHiringStageNameRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new GetHiringStageNameByIdHandler(_nameRepositoryMock.Object, _mapperMock.Object, null!);
    }

    [Fact]
    public async Task Handle_WithExistingStageName_ReturnsStageNameDTO()
    {
        // Arrange
        var query = new GetHiringStageNameByIdQuery(1);
        var stageName = new HiringStageName { Id = 1, Name = "Stage 1" };
        var stageNameDTO = new GetStageNameDTO { Id = 1, Name = "Stage 1" };

        _nameRepositoryMock.Setup(r => r.GetByIdAsync(query.Id))
            .ReturnsAsync(stageName);
        _mapperMock.Setup(m => m.Map<GetStageNameDTO>(stageName))
            .Returns(stageNameDTO);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(stageNameDTO, result);
        _nameRepositoryMock.Verify(r => r.GetByIdAsync(query.Id), Times.Once);
        _mapperMock.Verify(m => m.Map<GetStageNameDTO>(stageName), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistingStageName_ThrowsNoStageNameWithSuchIdException()
    {
        // Arrange
        var query = new GetHiringStageNameByIdQuery(1);

        _nameRepositoryMock.Setup(r => r.GetByIdAsync(query.Id))
            .ReturnsAsync((HiringStageName)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoStageNameWithSuchIdException>(() => _handler.Handle(query, CancellationToken.None));
        _nameRepositoryMock.Verify(r => r.GetByIdAsync(query.Id), Times.Once);
        _mapperMock.Verify(m => m.Map<GetStageNameDTO>(It.IsAny<HiringStageName>()), Times.Never);
    }
}
