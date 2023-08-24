using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementService.Application.CQRS.ProjectTaskCommands;
using ProjectManagementService.Application.CQRS.ProjectTaskQueries;
using ProjectManagementService.Application.ProjectTaskDTOs;

namespace ProjectManagementService.API.Controllers;

[Route("project-tasks")]
[ApiController]
public class ProjectTasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectTasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    //[Authorize(Roles = "DepartmentHead")]
    public async Task<IActionResult> GetAllAsync()
    {
        var tasks = await _mediator.Send(new GetAllProjectTasksQuery());
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    //[Authorize(Roles = "DepartmentHead")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
    {
        var task = await _mediator.Send(new GetProjectTaskByIdQuery(id));
        return Ok(task);
    }

    [HttpGet("projects/{id}")]
    //[Authorize(Roles = "DepartmentHead")]
    public async Task<IActionResult> GetByProjectIdAsync([FromRoute(Name = "id")] string projectId)
    {
        var tasks = await _mediator.Send(new GetProjectTasksByProjectIdQuery(projectId));
        return Ok(tasks);
    }

    [HttpGet("current")] 
    //[Authorize(Roles = "Worker,ProjectLeader")]
    public async Task<IActionResult> GetCurrentAsync() // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        //Worker id will be extracted from JWT
        return Ok();
    }

    [HttpPost]
    //[Authorize(Roles = "Customer")]
    public async Task<IActionResult> AddAsync(AddProjectTaskDTO taskDTO)
    {        
        string id = await _mediator.Send(new AddProjectTaskCommand(taskDTO)); 

        return Ok(id);
    }

    [HttpPut("cancel/{id}")]
    //[Authorize(Roles = "Customer")]
    public async Task<IActionResult> CancelAsync([FromRoute] string id, string customerId)// remove customerId from parameters
    {
        //Customer id will be extracted from JWT
        await _mediator.Send(new CancelProjectTaskByIdCommand(id, customerId));

        return NoContent();
    }

    [HttpPut("set-current-approvable")]
    //[Authorize(Roles = "Worker,ProjectLeader")]
    public async Task<IActionResult> MarkAsReadyToApproveAsync(string workerId)// remove workerId from parameters
    {
        //Worker id will be extracted from JWT

        await _mediator.Send(new MarkProjectTaskAsReadyToApproveCommand(workerId));

        return NoContent();
    }

    [HttpPut("aprove/{id}")]
    //[Authorize(Roles = "ProjectLeader")]
    public async Task<IActionResult> MarkAsApproved([FromRoute] string id, string projectLeaderId) // remove projectLeaderId from parameters
    {
        //ProjectLeader id will be extracted from JWT
        await _mediator.Send(new MarkProjectTaskAsApprovedCommand(id, projectLeaderId));

        return NoContent();
    }

    [HttpPut("start-current")]
    //[Authorize(Roles = "Worker")]
    public async Task<IActionResult> StartWorkingOnTask(string workerId)// remove workerId from parameters
    {
        await _mediator.Send(new StartWorkingOnTaskCommand(workerId));

        return NoContent();
    }

    [HttpPut("finish-current")]
    //[Authorize(Roles = "Worker")]
    public async Task<IActionResult> FinishWorkingOnTask(string workerId)// remove workerId from parameters
    {
        //Worker id will be extracted from JWT

        await _mediator.Send(new FinishWorkingOnTaskCommand(workerId));

        return NoContent();
    }
}
