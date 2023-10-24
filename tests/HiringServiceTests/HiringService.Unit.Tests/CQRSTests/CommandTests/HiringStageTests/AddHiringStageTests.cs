using HiringService.Application.CQRS.HiringStageCommands;
using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Application.Exceptions.Candidate;
using HiringService.Application.Exceptions.HiringStageName;
using HiringService.Application.Exceptions.Worker;

namespace HiringService.Unit.Tests.CQRSTests.CommandTests.HiringStageTests;

public class AddHiringStageTests
{
    private readonly Mock<IHiringStageRepository> _stageRepositoryMock;
    private readonly Mock<IHiringStageNameRepository> _nameRepositoryMock;
    private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly AddHiringStageHandler _handler;

    public AddHiringStageTests()
    {
        _stageRepositoryMock = new Mock<IHiringStageRepository>();
        _nameRepositoryMock = new Mock<IHiringStageNameRepository>();
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _mapperMock = new Mock<IMapper>();

        _handler = new AddHiringStageHandler(
            _stageRepositoryMock.Object,
            _workerRepositoryMock.Object,
            _mapperMock.Object,
            _candidateRepositoryMock.Object,
            null!, // IDistributedCache
            _nameRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_ValidRequest_ReturnsStageId()
    {
        // Arrange
        var stageDTO = new AddHiringStageDTO
        {
            HiringStageNameId = 1,
            CandidateId = 1,
            IntervierId = 1
        };

        var stageName = new HiringStageName();
        var candidate = new Candidate();
        var intervier = new Worker();
        var stage = new HiringStage { Id = 1 };

        _nameRepositoryMock.Setup(r => r.GetByIdAsync(stageDTO.HiringStageNameId))
            .ReturnsAsync(stageName);
        _candidateRepositoryMock.Setup(r => r.GetByIdAsync(stageDTO.CandidateId))
            .ReturnsAsync(candidate);
        _workerRepositoryMock.Setup(r => r.GetByIdAsync(stageDTO.IntervierId))
            .ReturnsAsync(intervier);
        _stageRepositoryMock.Setup(r => r.Add(It.IsAny<HiringStage>()))
            .Returns(stage);

        // Act
        var result = await _handler.Handle(new AddHiringStageCommand(stageDTO), CancellationToken.None);

        // Assert
        Assert.Equal(stage.Id, result);
        _stageRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidStageNameId_ThrowsNoStageNameWithSuchIdException()
    {
        // Arrange
        var stageDTO = new AddHiringStageDTO
        {
            HiringStageNameId = 1,
            CandidateId = 1,
            IntervierId = 1
        };

        _nameRepositoryMock.Setup(r => r.GetByIdAsync(stageDTO.HiringStageNameId))
            .ReturnsAsync((HiringStageName)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoStageNameWithSuchIdException>(() =>
            _handler.Handle(new AddHiringStageCommand(stageDTO), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidCandidateId_ThrowsNoCandidateWithSuchIdException()
    {
        // Arrange
        var stageDTO = new AddHiringStageDTO
        {
            HiringStageNameId = 1,
            CandidateId = 1,
            IntervierId = 1
        };

        _nameRepositoryMock.Setup(r => r.GetByIdAsync(stageDTO.HiringStageNameId))
            .ReturnsAsync(new HiringStageName());
        _candidateRepositoryMock.Setup(r => r.GetByIdAsync(stageDTO.CandidateId))
            .ReturnsAsync((Candidate)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoCandidateWithSuchIdException>(() =>
            _handler.Handle(new AddHiringStageCommand(stageDTO), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidIntervierId_ThrowsNoWorkerWithSuchIdException()
    {
        // Arrange
        var stageDTO = new AddHiringStageDTO
        {
            HiringStageNameId = 1,
            CandidateId = 1,
            IntervierId = 1
        };

        _nameRepositoryMock.Setup(r => r.GetByIdAsync(stageDTO.HiringStageNameId))
            .ReturnsAsync(new HiringStageName());
        _candidateRepositoryMock.Setup(r => r.GetByIdAsync(stageDTO.CandidateId))
            .ReturnsAsync(new Candidate());
        _workerRepositoryMock.Setup(r => r.GetByIdAsync(stageDTO.IntervierId))
            .ReturnsAsync((Worker)null!);

        // Act & Assert
        await Assert.ThrowsAsync<NoWorkerWithSuchIdException>(() =>
            _handler.Handle(new AddHiringStageCommand(stageDTO), CancellationToken.None));
    }
}

