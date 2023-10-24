using Confluent.Kafka;
using Newtonsoft.Json;
using ProjectManagementService.Application.ProjectDTOs;
using ProjectManagementService.Domain.Entities;
using ProjectManagementService.Domain.Enumerations;
using System.Net;
using System.Net.Http.Json;
using System.Text;

namespace ProjectManagementService.Integration.Tests.APITests.ControllerTests;

public class ProjectsControllerTests 
{
    private readonly HttpClient _client;
    private readonly JWTGenerator _JWTGenerator;

    public ProjectsControllerTests()
    {
        var application = new IdentityServiceWebApplicationFactory();
        _client = application.CreateClient();

        _JWTGenerator = new JWTGenerator();
    }

    [Fact]
    public async Task GetAllAsync_ReturnsOkResultWithProjects()
    {
        // Arrange
        var token = "Bearer " + _JWTGenerator.GetToken(
            ApplicationRole.DepartmentHead.ToString(),
            ""
        );

        var request = new HttpRequestMessage(HttpMethod.Get, "/projects");
        request.Headers.Add("Authorization", token);

        // Act
        var response = await _client.SendAsync(request);
        var projects = await response.Content.ReadFromJsonAsync<List<Project>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        projects.Should().NotBeNull();
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsOkResultWithProject()
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
        var request = new HttpRequestMessage(HttpMethod.Get, $"/projects/{projectId}");
        request.Headers.Add("Authorization", departmentHeadToken);

        // Act
        var response = await _client.SendAsync(request);
        var project = await response.Content.ReadFromJsonAsync<Project>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        project.Should().NotBeNull();
    }

    [Fact]
    public async Task GetAllCustomerProjectsAsync_ReturnsOkResultWithProjects()
    {
        var customerToken = "Bearer " + _JWTGenerator.GetToken(
            ApplicationRole.DepartmentHead.ToString(),
            "customer1@gmail.com"
        );

        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/projects/current-customer-projects");
        request.Headers.Add("Authorization", customerToken);

        // Act
        var response = await _client.SendAsync(request);
        var projects = await response.Content.ReadFromJsonAsync<List<Project>>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        projects.Should().NotBeNull();
    }

    [Fact]
    public async Task GetCustomerProjectByIdAsync_ReturnsOkResultWithProject()
    {
        var customerToken = "Bearer " + _JWTGenerator.GetToken(
            ApplicationRole.DepartmentHead.ToString(),
            "customer1@gmail.com"
        );

        var getAllRequest = new HttpRequestMessage(HttpMethod.Get, "/projects/current-customer-projects");
        getAllRequest.Headers.Add("Authorization", customerToken);

        var getAllResponse = await _client.SendAsync(getAllRequest);
        var projects = await getAllResponse.Content.ReadFromJsonAsync<List<ProjectShortInfoDTO>>();


        // Arrange
        var projectId = projects!.FirstOrDefault()!.Id;
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"/projects/current-customer-projects/{projectId}");

        // Act
        var response = await _client.SendAsync(request);
        var project = await response.Content.ReadFromJsonAsync<Project>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        project.Should().NotBeNull();
    }

    [Fact]
    public async Task GetProjectLeaderProject_ReturnsOkResultWithProject()
    {
        // Arrange
        var token = "Bearer " + _JWTGenerator.GetToken(
            ApplicationRole.ProjectLeader.ToString(),
            "plead@gmail.com"
        );
        var request = new HttpRequestMessage(HttpMethod.Get, "/projects/current");
        request.Headers.Add("Authorization", token);

        // Act
        var response = await _client.SendAsync(request);
        var project = await response.Content.ReadFromJsonAsync<Project>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        project.Should().NotBeNull();
    }

    [Fact]
    public async Task AddAsync_ReturnsOkResultWithId()
    {
        // Arrange
        var addProjectDTO = new AddProjectDTO
        {             
            Objective = "new",
            Description = "new project"
        };

        var serializedObject = new StringContent(
            JsonConvert.SerializeObject(addProjectDTO),
            Encoding.UTF8, "application/json");

        var customerToken = "Bearer " + _JWTGenerator.GetToken(
           ApplicationRole.DepartmentHead.ToString(),
           "customer1@gmail.com"
       );

        var request = new HttpRequestMessage(HttpMethod.Post, "/projects");
        request.Content = serializedObject;
        request.Headers.Add("Authorization", customerToken);

        // Act
        var response = await _client.SendAsync(request);
        var id = await response.Content.ReadFromJsonAsync<string>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        id.Should().NotBeNullOrEmpty();
    }
}
