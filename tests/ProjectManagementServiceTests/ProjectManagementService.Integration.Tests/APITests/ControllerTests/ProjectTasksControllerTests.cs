using ProjectManagementService.Application.TaskDTOs;
using System.Net.Http.Json;
using System.Net;
using ProjectManagementService.Domain.Enumerations;
using System.Text;
using Newtonsoft.Json;
using ProjectManagementService.Application.ProjectDTOs;
using System.Threading.Tasks;

namespace ProjectManagementService.Integration.Tests.APITests.ControllerTests;

public class ProjectTasksControllerTests
{ 
    private readonly HttpClient _client;
    private readonly JWTGenerator _JWTGenerator;

    public ProjectTasksControllerTests()
    {
        var application = new IdentityServiceWebApplicationFactory();
        _client = application.CreateClient();

        _JWTGenerator = new JWTGenerator();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOkResultWithTasks()
    {
        // Arrange
        var token = "Bearer " + _JWTGenerator.GetToken(
            ApplicationRole.DepartmentHead.ToString(),
            ""
        );

        var request = new HttpRequestMessage(HttpMethod.Get, "/project-tasks");
        request.Headers.Add("Authorization", token);

        // Act
        var response = await _client.SendAsync(request);
        var tasks = await response.Content.ReadFromJsonAsync<List<TaskShortInfoDTO>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        tasks.Should().NotBeNull();
        tasks.Should().HaveCountGreaterThan(0);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOkResultWithTask()
    {
        var departmentHeadToken = "Bearer " + _JWTGenerator.GetToken(
            ApplicationRole.DepartmentHead.ToString(),
            ""
        );

        var getAllRequest = new HttpRequestMessage(HttpMethod.Get, "/project-tasks");
        getAllRequest.Headers.Add("Authorization", departmentHeadToken);

        var getAllResponse = await _client.SendAsync(getAllRequest);
        var tasks = await getAllResponse.Content.ReadFromJsonAsync<List<TaskShortInfoDTO>>();


        // Arrange
        var getByIdRequest = new HttpRequestMessage(HttpMethod.Get, "/project-tasks");
        getByIdRequest.Headers.Add("Authorization", departmentHeadToken);

        // Act
        var taskId = tasks!.FirstOrDefault()!.Id;

        var getByIdResponse = await _client.GetAsync($"/project-tasks/{taskId}");
        var task = await getByIdResponse.Content.ReadFromJsonAsync<TaskMainInfoDTO>();

        // Assert
        getByIdResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        task.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByProjectIdAsync_ReturnsOkResultWithTasks()
    {
        var departmentHeadToken = "Bearer " + _JWTGenerator.GetToken(
            ApplicationRole.DepartmentHead.ToString(),
            ""
        );

        var getAllRequest = new HttpRequestMessage(HttpMethod.Get, "/projects");
        getAllRequest.Headers.Add("Authorization", departmentHeadToken);

        var getAllResponse = await _client.SendAsync(getAllRequest);
        var projects = await getAllResponse.Content.ReadFromJsonAsync<List<ProjectShortInfoDTO>>();


        // Arrange
        var projectId = projects!.FirstOrDefault()!.Id;

        // Act
        var response = await _client.GetAsync($"/project-tasks/projects/{projectId}");
        var tasks = await response.Content.ReadFromJsonAsync<List<TaskShortInfoDTO>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        tasks.Should().NotBeNull();
        tasks.Should().HaveCountGreaterThan(0);
        tasks.ForEach(task => task.ProjectId.Should().Be(projectId));
    }

    [Fact]
    public async Task GetCurrentAsync_ReturnsOkResultWithTask()
    {
        // Arrange
        var email = "worker1@gmail.com";

        var token = "Bearer " + _JWTGenerator.GetToken(
            ApplicationRole.Worker.ToString(),
            email
        );

        var request = new HttpRequestMessage(HttpMethod.Get, "/project-tasks/current");
        request.Headers.Add("Authorization", token);

        // Act
        var response = await _client.SendAsync(request);
        var task = await response.Content.ReadFromJsonAsync<TaskMainInfoDTO>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        task.Should().NotBeNull();
    }

    [Fact]
    public async Task AddAsync_ReturnsOkResultWithTaskId()
    {
        // Arrange
        var taskDTO = new AddProjectTaskDTO
        { 
            Name = "new",
            Description = "new task",
            Priority = Priority.Low,
        };

        var serializedObject = new StringContent(
            JsonConvert.SerializeObject(taskDTO),
            Encoding.UTF8, "application/json");

        var token = "Bearer " + _JWTGenerator.GetToken(
            ApplicationRole.Customer.ToString(),
            ""
        );

        var request = new HttpRequestMessage(HttpMethod.Post, "/project-tasks");
        request.Headers.Add("Authorization", token);
        request.Content = serializedObject;

        // Act
        var response = await _client.SendAsync(request);
        var taskId = await response.Content.ReadFromJsonAsync<string>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        taskId.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task CancelAsync_ReturnsNoContent()
    {
        var departmentHeadToken = "Bearer " + _JWTGenerator.GetToken(
            ApplicationRole.DepartmentHead.ToString(),
            ""
        );

        var getAllRequest = new HttpRequestMessage(HttpMethod.Get, "/project-tasks");
        getAllRequest.Headers.Add("Authorization", departmentHeadToken);

        var getAllResponse = await _client.SendAsync(getAllRequest);
        var tasks = await getAllResponse.Content.ReadFromJsonAsync<List<TaskShortInfoDTO>>();

        // Arrange
        var taskId = tasks!.FirstOrDefault()!.Id;

        var customerToken = "Bearer " + _JWTGenerator.GetToken(
            ApplicationRole.Customer.ToString(),
            ""
        );
        var request = new HttpRequestMessage(HttpMethod.Put, $"/project-tasks/cancel/{taskId}");
        request.Headers.Add("Authorization", customerToken);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task MarkAsReadyToApproveAsync_ReturnsNoContent()
    {
        // Arrange
        var email = "worker1@gmail.com";
        var token = "Bearer " + _JWTGenerator.GetToken(
            ApplicationRole.Worker.ToString(),
            email
        );
        var request = new HttpRequestMessage(HttpMethod.Put, $"/project-tasks/set-current-approvable");
        request.Headers.Add("Authorization", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task MarkAsApproved_ReturnsNoContent()
    {
        var departmentHeadToken = "Bearer " + _JWTGenerator.GetToken(
            ApplicationRole.ProjectLeader.ToString(),
            "plead@gmail.com"
        );

        var getAllRequest = new HttpRequestMessage(HttpMethod.Get, "/project-tasks");
        getAllRequest.Headers.Add("Authorization", departmentHeadToken);

        var getAllResponse = await _client.SendAsync(getAllRequest);
        var tasks = await getAllResponse.Content.ReadFromJsonAsync<List<TaskShortInfoDTO>>();

        // Arrange
        var taskId = tasks!.FirstOrDefault()!.Id;
        var token = "Bearer " + _JWTGenerator.GetToken(
            ApplicationRole.ProjectLeader.ToString(),
            "plead@gmail.com"
        );

        var request = new HttpRequestMessage(HttpMethod.Put, $"/project-tasks/approve/{taskId}");
        request.Headers.Add("Authorization", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task StartWorkingOnTask_ReturnsNoContent()
    {
        // Arrange
        var email = "worker1@gmail.com";
        var token = "Bearer " + _JWTGenerator.GetToken(
            ApplicationRole.Worker.ToString(),
            email
        );
        var request = new HttpRequestMessage(HttpMethod.Put, $"/project-tasks/start-current");
        request.Headers.Add("Authorization", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task FinishWorkingOnTask_ReturnsNoContent()
    {
        // Arrange
        var email = "worker1@gmail.com";
        var token = "Bearer " + _JWTGenerator.GetToken(
            ApplicationRole.Worker.ToString(),
            email
        );
        var request = new HttpRequestMessage(HttpMethod.Put, $"/project-tasks/finish-current");
        request.Headers.Add("Authorization", token);

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
