using HiringService.Application.Abstractions.ServiceAbstractions;
using HiringService.Application.CQRS.StageNameCommands;

namespace HiringService.Unit.Tests.CQRSTests.CommandTests.HiringStageNameTests;

public class RemoveStageNameTests
{
    private readonly Mock<IHiringStageNameRepository> _nameRepositoryMock;
    private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
    private readonly Mock<IHiringStageRepository> _stageRepositoryMock;
    private readonly Mock<IGRPCService> _gRPCServiceMock;
    private readonly Mock<IMapper> _mapperMock;

    private readonly RemoveStageNameHandler _handler;

    public RemoveStageNameTests()
    {
        _nameRepositoryMock = new Mock<IHiringStageNameRepository>();
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
        _stageRepositoryMock = new Mock<IHiringStageRepository>();
        _gRPCServiceMock = new Mock<IGRPCService>();
        _mapperMock = new Mock<IMapper>();

        _handler = new RemoveStageNameHandler(
            _nameRepositoryMock.Object,
            _stageRepositoryMock.Object,
            _candidateRepositoryMock.Object,
            null!, //cache
            _gRPCServiceMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_WithStagesToUpdate_UpdatesStagesAndShiftsIndexes()
    {
        // Arrange
        GetHiringStageNames(out HiringStageName stageName1, out HiringStageName stageName2);
        GetCandidates(out Candidate candidate1, out Candidate candidate2);
        GetHiringStages(
            stageName1, candidate1, candidate2,
            out HiringStage hiringStage1,
            out HiringStage hiringStage2
        );

        // remove first HiringStageName
        var request = new RemoveStageNameCommand(stageName1.Id);

        // test data for repositories storage (fake db)
        var stageNames = new List<HiringStageName> { stageName1, stageName2 };
        var candidates = new List<Candidate> { candidate1, candidate2 };
        var hiringStages = new List<HiringStage> { hiringStage1, hiringStage2 };

        _mapperMock.Setup(m => m.Map<HiringStageName>(null))
            .Returns((HiringStageName)null!);

        _nameRepositoryMock.Setup(r => r.GetByIdAsync(stageName1.Id))
            .ReturnsAsync(stageName1);

        _nameRepositoryMock.Setup(r => r.GetFilteredAsync(n => n.Index > stageName1.Index))
            .ReturnsAsync(stageNames.Where(n => n.Index > stageName1.Index).ToList());

        _nameRepositoryMock.Setup(r => r.Remove(stageName1))
            .Callback(() => stageNames.Remove(stageName1));

        _stageRepositoryMock.Setup(r => r.GetFilteredAsync(s => s.HiringStageNameId == stageName1.Id))
            .ReturnsAsync(hiringStages.Where(s => s.HiringStageNameId == stageName1.Id).ToList());

        _stageRepositoryMock.Setup(r => r.Remove(hiringStage1))
            .Callback(() => hiringStages.Remove(hiringStage1));

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        stageNames.Count.Should().Be(1); // stageName1 should be deleted
        hiringStages.Count.Should().Be(1); // hiringStage1 should be deleted
        stageName2.Index.Should().Be(0); // stageName2 should be reduced
        candidates.Count.Should().Be(2); // candidates should not be deleted

        _stageRepositoryMock.Verify(r => r.GetFilteredAsync(s => s.HiringStageNameId == stageName1.Id), Times.Once);
        _stageRepositoryMock.Verify(r => r.Remove(hiringStage1), Times.Once);
        _stageRepositoryMock.Verify(r => r.Update(hiringStage2), Times.Once);
        _stageRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);

        _nameRepositoryMock.Verify(r => r.GetByIdAsync(stageName1.Id), Times.Once);
        _nameRepositoryMock.Verify(r => r.Remove(stageName1), Times.Once);
        _nameRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Exactly(2));
        _nameRepositoryMock.Verify(r => r.Update(stageName2), Times.Once);
    }
    
    [Fact]
    public async Task Handle_NoMoreStagesToUpdate_DeletesCandidatesAndStages()
    {
        // Arrange
        GetHiringStageNames(out HiringStageName stageName1, out HiringStageName stageName2);
        GetCandidates(out Candidate candidate1, out Candidate candidate2);
        GetHiringStages(
            stageName2, candidate1, candidate2,
            out HiringStage hiringStage1,
            out HiringStage hiringStage2
        );

        // remove last HiringStageName
        var request = new RemoveStageNameCommand(stageName2.Id);

        // test data for repositories storage (fake db)
        var stageNames = new List<HiringStageName> { stageName1, stageName2 };
        var candidates = new List<Candidate> { candidate1, candidate2 };
        var hiringStages = new List<HiringStage> { hiringStage1, hiringStage2 };

        _mapperMock.Setup(m => m.Map<HiringStageName>(null))
            .Returns((HiringStageName)null!);

        _nameRepositoryMock.Setup(r => r.GetByIdAsync(stageName2.Id))
            .ReturnsAsync(stageName2);

        _nameRepositoryMock.Setup(r => r.GetFilteredAsync(n => n.Index > stageName2.Index))
            .ReturnsAsync(stageNames.Where(n => n.Index > stageName2.Index).ToList()); //enpty list

        _nameRepositoryMock.Setup(r => r.Remove(stageName2))
            .Callback(() => stageNames.Remove(stageName2));

        _stageRepositoryMock.Setup(r => r.GetFilteredAsync(s => s.HiringStageNameId == stageName2.Id))
            .ReturnsAsync(hiringStages.Where(s => s.HiringStageNameId == stageName2.Id).ToList());

        _candidateRepositoryMock.Setup(r => r.GetByIdAsync(candidate1.Id))
            .ReturnsAsync(candidate1);
        _candidateRepositoryMock.Setup(r => r.GetByIdAsync(candidate2.Id))
            .ReturnsAsync(candidate2);

        _candidateRepositoryMock.Setup(r => r.RemoveRange(candidates))
            .Callback(() =>
            {
                candidates.Clear();
                hiringStages.Clear();
            });

        // Act
        await _handler.Handle(request, CancellationToken.None);

        // Assert
        stageNames.Count.Should().Be(1); // stageName2 should be deleted
        hiringStages.Count.Should().Be(0); // all HiringStages should be deleted
        candidates.Count.Should().Be(0); // all candidates should be deleted

        _stageRepositoryMock.Verify(r => r.GetFilteredAsync(s => s.HiringStageNameId == stageName2.Id), Times.Once);
        _stageRepositoryMock.Verify(r => r.Remove(hiringStage1), Times.Never);
        _stageRepositoryMock.Verify(r => r.Update(hiringStage2), Times.Never);
        _stageRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);

        _nameRepositoryMock.Verify(r => r.GetByIdAsync(stageName2.Id), Times.Once);
        _nameRepositoryMock.Verify(r => r.Remove(stageName2), Times.Once);
        _nameRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        _nameRepositoryMock.Verify(r => r.Update(stageName2), Times.Never);

        _candidateRepositoryMock.Verify(r => r.RemoveRange(It.IsAny<List<Candidate>>()), Times.Once);
        _candidateRepositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
    }

    private void GetHiringStageNames(out HiringStageName stageName1, out HiringStageName stageName2)
    {
        stageName1 = new HiringStageName()
        {
            Id = 1,
            Name = "1",
            Index = 0
        };
        stageName2 = new HiringStageName()
        {
            Id = 2,
            Name = "2",
            Index = 1
        };
    }

    private void GetCandidates(out Candidate candidate1, out Candidate candidate2)
    {
        candidate1 = new Candidate() { Id = 1 };
        candidate2 = new Candidate() { Id = 2 };
    }

    private void GetHiringStages(
        HiringStageName stageName1,
        Candidate candidate1,
        Candidate candidate2,
        out HiringStage hiringStage1,
        out HiringStage hiringStage2)
    {
        hiringStage1 = new HiringStage()
        {
            Id = 1,
            HiringStageName = stageName1,
            HiringStageNameId = stageName1.Id,
            Candidate = candidate1,
            CandidateId = candidate1.Id,
            PassedSuccessfully = true
        };
        hiringStage2 = new HiringStage()
        {
            Id = 2,
            HiringStageName = stageName1,
            HiringStageNameId = stageName1.Id,
            Candidate = candidate2,
            CandidateId = candidate2.Id,
            PassedSuccessfully = false
        };
    }
}
