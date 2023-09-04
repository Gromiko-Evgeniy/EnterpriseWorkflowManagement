using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementService.Application.Abstractions.ServiceAbstractions;
using ProjectManagementService.Application.CQRS.CustomerQueries;
using ProjectManagementService.Application.CQRS.ProjectTaskCommands;
using ProjectManagementService.Application.CQRS.ProjectTaskQueries;
using ProjectManagementService.Application.CQRS.WorkerQueries;
using ProjectManagementService.Application.ProjectTaskDTOs;
using ProjectManagementService.Domain.Enumerations;

namespace ProjectManagementService.API.Controllers;

[Route("project-tasks")]
[ApiController]
public class ProjectTasksController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IJWTExtractorService _JWTExtractorService;
    private const string _depHeadRole = nameof(ApplicationRole.DepartmentHead);
    private const string _workerRole = nameof(ApplicationRole.Worker);
    private const string _customerRole = nameof(ApplicationRole.Customer);
    private const string _leaderRole = nameof(ApplicationRole.ProjectLeader);

    public ProjectTasksController(IMediator mediator, IJWTExtractorService JWTExtractorService)
    {
        _mediator = mediator;
        _JWTExtractorService = JWTExtractorService;
    }

    [HttpGet]
    [Authorize(Roles = _depHeadRole)]
    public async Task<IActionResult> GetAllAsync()
    {
        var tasks = await _mediator.Send(new GetAllProjectTasksQuery());

        return Ok(tasks);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = _depHeadRole)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
    {
        var task = await _mediator.Send(new GetProjectTaskByIdQuery(id));

        return Ok(task);
    }

    [HttpGet("projects/{id}")]
    [Authorize(Roles = _depHeadRole)]
    public async Task<IActionResult> GetByProjectIdAsync([FromRoute(Name = "id")] string projectId)
    {
        var tasks = await _mediator.Send(new GetProjectTasksByProjectIdQuery(projectId));

        return Ok(tasks);
    }

    [HttpGet("current")] 
    [Authorize(Roles = $"{_workerRole},{_leaderRole}")]
    public async Task<IActionResult> GetCurrentAsync()
    {
        var email = _JWTExtractorService.ExtractClaim(HttpContext.Request, "email");

        var worker = await _mediator.Send(new GetWorkerByEmailQuery(email));

        var task = await _mediator.Send(new GetProjectTaskByWorkerIdQuery(worker.Id));

        return Ok(task);
    }

    [HttpPost]
    [Authorize(Roles = _customerRole)]
    public async Task<IActionResult> AddAsync(AddProjectTaskDTO taskDTO)
    {        
        var id = await _mediator.Send(new AddProjectTaskCommand(taskDTO)); 

        return Ok(id);
    }

    [HttpPut("cancel/{id}")]
    [Authorize(Roles = _customerRole)]
    public async Task<IActionResult> CancelAsync([FromRoute] string id)
    {
        var email = _JWTExtractorService.ExtractClaim(HttpContext.Request, "email");

        var customer = await _mediator.Send(new GetCustomerByEmailQuery(email));

        await _mediator.Send(new CancelProjectTaskByIdCommand(id, customer.Id));

        return NoContent();
    }

    [HttpPut("set-current-approvable")]
    [Authorize(Roles = $"{_workerRole},{_leaderRole}")]
    public async Task<IActionResult> MarkAsReadyToApproveAsync()
    {
        var email = _JWTExtractorService.ExtractClaim(HttpContext.Request, "email");

        var worker = await _mediator.Send(new GetWorkerByEmailQuery(email));

        await _mediator.Send(new MarkProjectTaskAsReadyToApproveCommand(worker.Id));

        return NoContent();
    }

    [HttpPut("approve/{id}")]
    [Authorize(Roles = _leaderRole)]
    public async Task<IActionResult> MarkAsApproved([FromRoute] string id)
    {
        var email = _JWTExtractorService.ExtractClaim(HttpContext.Request, "email");

        var projectLeader = await _mediator.Send(new GetWorkerByEmailQuery(email));
        
        await _mediator.Send(new MarkProjectTaskAsApprovedCommand(id, projectLeader.Id));

        return NoContent();
    }

    [HttpPut("start-current")]
    [Authorize(Roles = _workerRole)]
    public async Task<IActionResult> StartWorkingOnTask()
    { 
        var email = _JWTExtractorService.ExtractClaim(HttpContext.Request, "email");

        var worker = await _mediator.Send(new GetWorkerByEmailQuery(email));

        await _mediator.Send(new StartWorkingOnTaskCommand(worker.Id));

        return NoContent();
    }

    [HttpPut("finish-current")]
    [Authorize(Roles = _workerRole)]
    public async Task<IActionResult> FinishWorkingOnTask()
    {
        var email = _JWTExtractorService.ExtractClaim(HttpContext.Request, "email");

        var worker = await _mediator.Send(new GetWorkerByEmailQuery(email));

        await _mediator.Send(new FinishWorkingOnTaskCommand(worker.Id));

        return NoContent();
    }
}
