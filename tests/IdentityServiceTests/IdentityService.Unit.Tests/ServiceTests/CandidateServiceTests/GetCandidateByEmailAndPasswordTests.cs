using IdentityService.Application.DTOs;
using IdentityService.Application.Exceptions;
using IdentityService.Application.Exceptions.Candidate;

namespace IdentityService.Unit.Tests.ServiceTests.CandidateServiceTests;

public class GetCandidateByEmailAndPasswordTests
{
    private readonly Mock<ICandidateRepository> _candidateRepositoryMock;
    private readonly CandidateService _candidateService;

    public GetCandidateByEmailAndPasswordTests()
    {
        _candidateRepositoryMock = new Mock<ICandidateRepository>();
        _candidateService = new CandidateService(_candidateRepositoryMock.Object, null!, null!);
    }

    [Fact]
    public async Task GetByEmailAndPasswordAsync_WhenCandidateExistsAndPasswordsMatch_ReturnsCandidate()
    {
        // Arrange
        var candidate = new Candidate
        { 
            Email = "test@example.com",
            Password = "password123" 
        };
        var logInData = new LogInData 
        { 
            Email = candidate.Email,
            Password = candidate.Password 
        };

        _candidateRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Candidate, bool>>>()))
            .ReturnsAsync(candidate);

        // Act
        var result = await _candidateService.GetByEmailAndPasswordAsync(logInData);

        // Assert
        result.Should().BeEquivalentTo(candidate);
    }

    [Fact]
    public async Task GetByEmailAndPasswordAsync_WhenCandidateDoesNotExist_ThrowsNoCandidateWithSuchEmailException()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var logInData = new LogInData { Email = email, Password = password };

        _candidateRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Candidate, bool>>>()))
            .ReturnsAsync((Candidate)null!);

        // Act
        Func<Task> act = async () => await _candidateService.GetByEmailAndPasswordAsync(logInData);

        // Assert
        await act.Should().ThrowAsync<NoCandidateWithSuchEmailException>();
    }

    [Fact]
    public async Task GetByEmailAndPasswordAsync_WhenPasswordsDoNotMatch_ThrowsIncorrectPasswordException()
    {
        // Arrange
        var candidate = new Candidate { Email = "test@example.com", Password = "password123" };
        var logInData = new LogInData { Email = candidate.Email, Password = "wrongpassword" };

        _candidateRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Candidate, bool>>>()))
            .ReturnsAsync(candidate);

        // Act
        Func<Task> act = async () => await _candidateService.GetByEmailAndPasswordAsync(logInData);

        // Assert
        await act.Should().ThrowAsync<IncorrectPasswordException>();
    }
}

