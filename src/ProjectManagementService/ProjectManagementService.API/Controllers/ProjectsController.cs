using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementService.Application.CQRS.ProjectCommands;
using ProjectManagementService.Application.CQRS.ProjectQueries;
using ProjectManagementService.Application.ProjectDTOs;
using ProjectManagementService.Domain.Enumerations;

namespace ProjectManagementService.API.Controllers;

[Route("projects")]
[ApiController]
public class ProjectsController : ControllerBase
{
    private readonly IMediator _mediator;
    private const string _depHeadRole = nameof(ApplicationRole.DepartmentHead);
    private const string _customerRole = nameof(ApplicationRole.Customer);
    private const string _leaderRole = nameof(ApplicationRole.ProjectLeader);

    public ProjectsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    //[Authorize(Roles = _depHeadRole)]
    public async Task<IActionResult> GetAllAsync() 
    {
        var projects = await _mediator.Send(new GetAllProjectsQuery());

        return Ok(projects);
    }

    [HttpGet("{id}")]
    //[Authorize(Roles = _depHeadRole)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
    {
        var project = await _mediator.Send(new GetProjectByIdQuery(id));

        return Ok(project);
    }

    [HttpGet("current-customer-projects")]
    //[Authorize(Roles = _customerRole)]
    public async Task<IActionResult> GetAllCustomerProjectsAsync(string customerId) // remove customerId from parameters
    {
        //Customer id will be extracted from JWT
        //var customerId = "";

        var project = await _mediator.Send(new GetAllCustomerProjectsQuery(customerId));

        return Ok(project); 
    }

    [HttpGet("current-customer-projects/{id}")] 
    //[Authorize(Roles = _customerRole)]
    public async Task<IActionResult> GetCustomerProjectByIdAsync([FromRoute] string id, string customerId)// remove customerId from parameters
    {
        var project = await _mediator.Send(new GetCustomerProjectByIdQuery(customerId, id));

        return Ok(project);
    }

    [HttpGet("current")]
    //[Authorize(Roles = _leaderRole)]
    public async Task<IActionResult> GetProjectLeaderProject(string projectLeaderId)// remove projectLeaderId from parameters
    {
        // ProjectLeader id will be extracted from JWT

        var project = await _mediator.Send(new GetProjectByLeaderIdQuery(projectLeaderId));

        return Ok(project);
    }

    [HttpPost]
    //[Authorize(Roles = _customerRole)]
    public async Task<IActionResult> AddAsync(AddProjectDTO addProjectDTO, string customerId)// remove customerId from parameters
    {
        //Customer id will be extracted from JWT
        var id = await _mediator.Send(new AddProjectCommand(addProjectDTO, customerId));

        return Ok(id);
    }
}
