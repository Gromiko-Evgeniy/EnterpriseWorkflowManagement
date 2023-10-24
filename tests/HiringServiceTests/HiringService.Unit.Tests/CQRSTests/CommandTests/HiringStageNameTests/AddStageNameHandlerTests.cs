using HiringService.Application.CQRS.StageNameCommands;
using HiringService.Application.DTOs.StageNameDTOs;
using HiringService.Application.Exceptions.HiringStageName;

namespace HiringService.Unit.Tests.CQRSTests.CommandTests.HiringStageNameTests;

public class AddStageNameHandlerTests
{
    private readonly Mock<IHiringStageNameRepository> _nameRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AddStageNameHandler _handler;

    public AddStageNameHandlerTests()
    {
        _nameRepositoryMock = new Mock<IHiringStageNameRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new AddStageNameHandler(
            _nameRepositoryMock.Object,
            null!, // cache 
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithValidData_ReturnsStageNameId()
    {
        // Arrange
        var stageNameDTO = new AddStageNameDTO() 
        {
            Name = "New",
            Index = 666
        };
        var newStageName = new HiringStageName() 
        {
            Name = "New",
            Index = 666,
            Id = 666
        };

        var command = new AddStageNameCommand(stageNameDTO);

        _nameRepositoryMock.Setup(r => r.GetByNameAsync(stageNameDTO.Name))
            .ReturnsAsync((HiringStageName)null!); // not no same stage name in database

        _nameRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new List<HiringStageName>());

        _mapperMock.Setup(m => m.Map<HiringStageName>(stageNameDTO))
            .Returns(newStageName);

        _nameRepositoryMock.Setup(r => r.Add(newStageName))
            .Returns(newStageName);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(newStageName.Id);
    }

    [Fact]
    public async Task Handle_WithExistingStageName_ThrowsStageNameAlreadyExistsException()
    {
        // Arrange
        var stageNameDTO = new AddStageNameDTO() { Name = "New" };
        var command = new AddStageNameCommand(stageNameDTO);

        var existingStageName = new HiringStageName() { Name = "New" };

        _nameRepositoryMock.Setup(r => r.GetByNameAsync(stageNameDTO.Name))
            .ReturnsAsync(existingStageName);

        // Act & Assert
        await Assert.ThrowsAsync<StageNameAlreadyExistsException>(
            () => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_WithExistingIndex_ShiftsIndicesOfSubsequentElements()
    {
        // Arrange
        var stageNameDTO = new AddStageNameDTO() { Name = "New", Index = 1 };
        var newStageName = new HiringStageName() { Name = "New", Index = 1 };

        var command = new AddStageNameCommand(stageNameDTO);

        var tageName0 = new HiringStageName() { Index = 0 };
        var tageName1 = new HiringStageName() { Index = 1 };
        var tageName2 = new HiringStageName() { Index = 2 };

        var stageNames = new List<HiringStageName>
        {
            tageName0,
            tageName1,
            tageName2
        };

        _nameRepositoryMock.Setup(r => r.GetByNameAsync(stageNameDTO.Name))
            .ReturnsAsync((HiringStageName)null!); // not no same stage name in database

        _nameRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(stageNames);

        _mapperMock.Setup(m => m.Map<HiringStageName>(stageNameDTO))
            .Returns(newStageName);

        _nameRepositoryMock.Setup(r => r.Add(newStageName))
            .Callback(() => stageNames.Add(newStageName))
            .Returns(newStageName);
        
        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        stageNames.Count.Should().Be(4); // new stage name should be added
        tageName0.Index.Should().Be(0); // tageName0 index should not be shifted
        tageName1.Index.Should().Be(2); // tageName1 index should be shifted
        tageName2.Index.Should().Be(3); // tageName2 index should be shifted
        newStageName.Index.Should().Be(1); // newStageName index should remain the same
    }

    [Fact]
    public async Task Handle_WithoutExistingIndex_SetsIndexToMaxIndexPlusOne()
    {
        // Arrange
        var stageNameDTO = new AddStageNameDTO() { Name = "New", Index = 666 };
        var newStageName = new HiringStageName() { Name = "New", Index = 666 };

        var command = new AddStageNameCommand(stageNameDTO);

        var tageName0 = new HiringStageName() { Index = 0 };
        var tageName1 = new HiringStageName() { Index = 1 };
        var tageName2 = new HiringStageName() { Index = 2 };

        var stageNames = new List<HiringStageName>
        {
            tageName0,
            tageName1,
            tageName2
        };

        _nameRepositoryMock.Setup(r => r.GetByNameAsync(stageNameDTO.Name))
            .ReturnsAsync((HiringStageName)null!); // not no same stage name in database

        _nameRepositoryMock.Setup(r => r.GetAllAsync())
            .ReturnsAsync(stageNames);

        _mapperMock.Setup(m => m.Map<HiringStageName>(stageNameDTO))
            .Returns(newStageName);

        _nameRepositoryMock.Setup(r => r.Add(newStageName))
            .Callback(() => stageNames.Add(newStageName))
            .Returns(newStageName);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        stageNames.Count.Should().Be(4); // new stage name should be added
        tageName0.Index.Should().Be(0); // tageName0 index should not be shifted
        tageName1.Index.Should().Be(1); // tageName1 index should be shifted
        tageName2.Index.Should().Be(2); // tageName2 index should be shifted
        newStageName.Index.Should().Be(3); // newStageName is set to max index + 1 
    }
}
