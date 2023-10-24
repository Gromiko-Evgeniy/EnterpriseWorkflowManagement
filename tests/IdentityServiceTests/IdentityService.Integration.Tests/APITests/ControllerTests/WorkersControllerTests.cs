using IdentityService.Application.DTOs;
using System.Net.Http.Json;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using IdentityService.Domain.Enumerations;
using Azure;

namespace IdentityService.Integration.Tests.APITests.ControllerTests;

public class WorkersControllerTests
{
    private readonly HttpClient _client;
    private readonly JWTGenerator _JWTGenerator;

    public WorkersControllerTests()
    {
        var application = new IdentityServiceWebApplicationFactory();
        _client = application.CreateClient();

        _JWTGenerator = new JWTGenerator();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOkResultWithWorkerDTOs()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/workers");
        request.Headers.Add("Authorization",
            "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.DepartmentHead.ToString(),
                ""
            )
        );

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task GetByEmailAsync_ReturnsOkResultWithWorkerDTO()
    {
        // Arrange
        var email = "worker1@gmail.com";

        var request = new HttpRequestMessage(HttpMethod.Get, $"/workers/{email}");
        request.Headers.Add("Authorization",
            "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.DepartmentHead.ToString(),
                ""
            )
        );

        // Act
        var response = await _client.SendAsync(request);
        var worker = await response.Content.ReadFromJsonAsync<Worker>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        worker?.Email.Should().Be(email);
    }

    [Fact]
    public async Task GetCurrentAsync_ReturnsOkResultWithWorkerDTO()
    {
        // Arrange
        var email = "worker1@gmail.com";

        var request = new HttpRequestMessage(HttpMethod.Get, "/workers/current");
        request.Headers.Add("Authorization",
            "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.DepartmentHead.ToString(),
                email
            )
        );

        // Act
        var response = await _client.SendAsync(request);
        var worker = await response.Content.ReadFromJsonAsync<Worker>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        worker?.Email.Should().Be(email);
    }

    [Fact]
    public async Task LogInAsync_ReturnsOkResultWithToken()
    {
        // Arrange
        var logInData = new LogInData
        { 
            Email = "worker1@gmail.com",
            Password = "1234"
        };

        var serializedObject = new StringContent(
            JsonConvert.SerializeObject(logInData),
            Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, "/workers/log-in");
        request.Content = serializedObject;

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UpdateNameAsync_ReturnsNoContent()
    {
        // Arrange
        var name = "New Name";
        var email = "worker1@gmail.com";

        var token = "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.Worker.ToString(),
                email);

        var updateRequest = new HttpRequestMessage(HttpMethod.Put, "/workers/new-name");

        updateRequest.Content = new StringContent(
            JsonConvert.SerializeObject(name),
            Encoding.UTF8,
            "application/json");

        updateRequest.Headers.Add("Authorization", token);

        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/workers/current");
        getRequest.Headers.Add("Authorization", token);

        // Act
        var updateResponse = await _client.SendAsync(updateRequest);
        var getResponse = await _client.SendAsync(getRequest);

        var worker = await getResponse.Content.ReadFromJsonAsync<Worker>();

        // Assert
        updateResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        worker?.Email.Should().Be(email);
        worker?.Name.Should().Be(name);
    }

    [Fact]
    public async Task PromoteAsync_ReturnsNoContent()
    {
        // Arrange
        var email = "worker1@gmail.com";
        var request = new HttpRequestMessage(HttpMethod.Put, $"/workers/promote/{email}");
        request.Headers.Add("Authorization",
            "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.DepartmentHead.ToString(),
                email
            )
        );

        // Act
        var response = await _client.SendAsync(request);
        var worker = await response.Content.ReadFromJsonAsync<Worker>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        worker?.Position.Should().Be(Position.ProjectLeader);

    }

    [Fact]
    public async Task DemoteAsync_ReturnsNoContent()
    {
        // Arrange
        var email = "plead@gmail.com";
        var token = "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.DepartmentHead.ToString(),
                email
            );

        var request = new HttpRequestMessage(HttpMethod.Put, $"/workers/demote/{email}");
        request.Headers.Add("Authorization", token);

        // Act
        var response = await _client.SendAsync(request);
        var worker = await response.Content.ReadFromJsonAsync<Worker>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        worker?.Position.Should().Be(Position.Worker);
    }

    [Fact]
    public async Task DismissWorkerAsync_ReturnsNoContent()
    {
        // Arrange
        var email = "worker1@gmail.com";

        var token = "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.DepartmentHead.ToString(),
                email);

        var dismissRequest = new HttpRequestMessage(HttpMethod.Delete, $"/workers/dismiss/{email}");
        dismissRequest.Headers.Add("Authorization", token);

        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/workers/current");
        getRequest.Headers.Add("Authorization", token);

        // Act
        var dismissResponse = await _client.SendAsync(dismissRequest);
        var getResponse = await _client.SendAsync(getRequest);

        // Assert
        dismissResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task QuitAsync_ReturnsNoContent()
    {
        // Arrange
        var email = "worker1@gmail.com";
        var password = "1234";

        var workerToken = "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.Worker.ToString(),
                email
            );
        var departmentHeadToken = "Bearer " + _JWTGenerator.GetToken(
                ApplicationRole.DepartmentHead.ToString(),
                email
            );

        var quitRequest = new HttpRequestMessage(
            HttpMethod.Delete,
            $"/workers/quit?password={password}");
        quitRequest.Headers.Add("Authorization", workerToken);

        var getRequest = new HttpRequestMessage(HttpMethod.Get, "/workers/current");
        getRequest.Headers.Add("Authorization", departmentHeadToken);

        // Act
        var quitResponse = await _client.SendAsync(quitRequest);
        var getResponse = await _client.SendAsync(getRequest);

        // Assert
        quitResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
        getResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
