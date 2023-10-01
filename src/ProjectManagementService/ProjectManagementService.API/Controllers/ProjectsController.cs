using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementService.Application.Abstractions.ServiceAbstractions;
using ProjectManagementService.Application.CQRS.CustomerQueries;
using ProjectManagementService.Application.CQRS.ProjectCommands;
using ProjectManagementService.Application.CQRS.ProjectQueries;
using ProjectManagementService.Application.CQRS.WorkerQueries;
using ProjectManagementService.Application.ProjectDTOs;
using ProjectManagementService.Domain.Enumerations;

namespace ProjectManagementService.API.Controllers;

[Route("projects")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IJWTExtractorService _JWTExtractorService;
    private const string _depHeadRole = nameof(ApplicationRole.DepartmentHead);
    private const string _customerRole = nameof(ApplicationRole.Customer);
    private const string _leaderRole = nameof(ApplicationRole.ProjectLeader);

    public ProjectsController(IMediator mediator, IJWTExtractorService JWTExtractorService)
    {
        _mediator = mediator;
        _JWTExtractorService = JWTExtractorService;
    }

    [HttpGet]
    [Authorize(Roles = _depHeadRole)]
    public async Task<IActionResult> GetAllAsync() 
    {
        var projects = await _mediator.Send(new GetAllProjectsQuery());

        return Ok(projects);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = _depHeadRole)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
    {
        var project = await _mediator.Send(new GetProjectByIdQuery(id));

        return Ok(project);
    }

    [HttpGet("current-customer-projects")]
    [Authorize(Roles = _customerRole)]
    public async Task<IActionResult> GetAllCustomerProjectsAsync()
    {
        var email = _JWTExtractorService.ExtractClaimFromRequest(HttpContext.Request, "email");

        var customer = await _mediator.Send(new GetCustomerByEmailQuery(email));

        var project = await _mediator.Send(new GetAllCustomerProjectsQuery(customer.Id));

        return Ok(project); 
    }

    [HttpGet("current-customer-projects/{id}")] 
    [Authorize(Roles = _customerRole)]
    public async Task<IActionResult> GetCustomerProjectByIdAsync([FromRoute] string id)
    {
        var email = _JWTExtractorService.ExtractClaimFromRequest(HttpContext.Request, "email");

        var customer = await _mediator.Send(new GetCustomerByEmailQuery(email));

        var project = await _mediator.Send(new GetCustomerProjectByIdQuery(customer.Id, id));

        return Ok(project);
    }

    [HttpGet("current")]
    [Authorize(Roles = _leaderRole)]
    public async Task<IActionResult> GetProjectLeaderProject()
    {
        var email = _JWTExtractorService.ExtractClaimFromRequest(HttpContext.Request, "email");

        var projectLeader = await _mediator.Send(new GetWorkerByEmailQuery(email));

        var project = await _mediator.Send(new GetProjectByLeaderIdQuery(projectLeader.Id));

        return Ok(project);
    }

    [HttpPost]
    [Authorize(Roles = _customerRole)]
    public async Task<IActionResult> AddAsync(AddProjectDTO addProjectDTO)
    {
        var email = _JWTExtractorService.ExtractClaimFromRequest(HttpContext.Request, "email");

        var customer = await _mediator.Send(new GetCustomerByEmailQuery(email));

        var id = await _mediator.Send(new AddProjectCommand(addProjectDTO, customer.Id));

        return Ok(id);
    }
}
