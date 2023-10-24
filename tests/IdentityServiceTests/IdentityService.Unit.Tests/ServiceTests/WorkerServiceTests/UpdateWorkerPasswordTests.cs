using IdentityService.Application.Exceptions;
using IdentityService.Application.Exceptions.Worker;

namespace IdentityService.Unit.Tests.ServiceTests.WorkerServiceTests;

public class UpdateWorkerPasswordTests
{
    private readonly Mock<IWorkerRepository> _workerRepositoryMock;
    private readonly WorkerService _workerService;

    public UpdateWorkerPasswordTests()
    {
        _workerRepositoryMock = new Mock<IWorkerRepository>();
        _workerService = new WorkerService(_workerRepositoryMock.Object, null!, null!);
    }

    [Fact]
    public async Task UpdatePasswordAsync_WhenValidCredentials_UpdatesPassword()
    {
        // Arrange
        var email = "test@example.com";
        var prevPassword = "oldpassword";
        var newPassword = "newpassword";
        var worker = new Worker { Email = email, Password = prevPassword };

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync(worker);

        // Act
        await _workerService.UpdatePasswordAsync(email, prevPassword, newPassword);

        // Assert
        worker.Password.Should().Be(newPassword);
        _workerRepositoryMock.Verify(repo => repo.Update(worker), Times.Once);
        _workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdatePasswordAsync_WhenInvalidCredentials_ThrowsIncorrectPasswordException()
    {
        // Arrange
        var email = "test@example.com";
        var prevPassword = "oldpassword";
        var newPassword = "newpassword";
        var worker = new Worker { Email = email, Password = "wrongpassword" };

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync(worker);

        // Act
        Func<Task> act = async () => await _workerService.UpdatePasswordAsync(email, prevPassword, newPassword);

        // Assert
        await act.Should().ThrowAsync<IncorrectPasswordException>();
        _workerRepositoryMock.Verify(repo => repo.Update(worker), Times.Never);
        _workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdatePasswordAsync_WhenWorkerDoesNotExist_ThrowsNoWorkerWithSuchEmailException()
    {
        // Arrange
        var email = "test@example.com";
        var prevPassword = "oldpassword";
        var newPassword = "newpassword";

        _workerRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Worker, bool>>>()))
            .ReturnsAsync((Worker)null!);

        // Act
        Func<Task> act = async () => await _workerService.UpdatePasswordAsync(email, prevPassword, newPassword);

        // Assert
        await act.Should().ThrowAsync<NoWorkerWithSuchEmailException>();
        _workerRepositoryMock.Verify(repo => repo.Update(It.IsAny<Worker>()), Times.Never);
        _workerRepositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
    }
}
