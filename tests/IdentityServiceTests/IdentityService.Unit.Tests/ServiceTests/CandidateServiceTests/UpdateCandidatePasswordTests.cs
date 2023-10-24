using IdentityService.Application.Exceptions;
using IdentityService.Application.Exceptions.Candidate;
using IdentityService.Application.RepositoryAbstractions;

namespace IdentityService.Unit.Tests.ServiceTests.CandidateServiceTests;

public class UpdateCandidatePasswordTests
{
    private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
    private readonly CandidateService _candidateService;

    public UpdateCandidatePasswordTests()
    {
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
        _candidateService = new CandidateService(_candidateRepositoryMock.Object, null!, null!);
    }

    [Fact]
    public async Task UpdatePasswordAsync_WhenCandidateExistsAndPasswordsMatch_UpdatesPassword()
    {
        // Arrange
        var email = "test@example.com";
        var prevPassword = "password123";
        var newPassword = "newpassword456";
        var candidate = new Candidate { Email = email, Password = prevPassword };

        _candidateRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Candidate, bool>>>()))
            .ReturnsAsync(candidate);

        // Act
        await _candidateService.UpdatePasswordAsync(email, prevPassword, newPassword);

        // Assert
        candidate.Password.Should().Be(newPassword);
        _candidateRepositoryMock.Verify(repo => repo.Update(candidate), Times.Once);
        _candidateRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdatePasswordAsync_WhenCandidateDoesNotExist_ThrowsNoCandidateWithSuchEmailException()
    {
        // Arrange
        var email = "test@example.com";
        var prevPassword = "password123";
        var newPassword = "newpassword456";

        _candidateRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Candidate, bool>>>()))
            .ReturnsAsync((Candidate)null);

        // Act
        Func<Task> act = async () => await _candidateService.UpdatePasswordAsync(email, prevPassword, newPassword);

        // Assert
        await act.Should().ThrowAsync<NoCandidateWithSuchEmailException>();
        _candidateRepositoryMock.Verify(repo => repo.Update(It.IsAny<Candidate>()), Times.Never);
        _candidateRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdatePasswordAsync_WhenPasswordsDoNotMatch_ThrowsIncorrectPasswordException()
    {
        // Arrange
        var email = "test@example.com";
        var prevPassword = "password123";
        var newPassword = "newpassword456";
        var candidate = new Candidate { Email = email, Password = "wrongpassword" };

        _candidateRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Candidate, bool>>>()))
            .ReturnsAsync(candidate);

        // Act
        Func<Task> act = async () => await _candidateService.UpdatePasswordAsync(email, prevPassword, newPassword);

        // Assert
        await act.Should().ThrowAsync<IncorrectPasswordException>();
        _candidateRepositoryMock.Verify(repo => repo.Update(It.IsAny<Candidate>()), Times.Never);
        _candidateRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }
}

