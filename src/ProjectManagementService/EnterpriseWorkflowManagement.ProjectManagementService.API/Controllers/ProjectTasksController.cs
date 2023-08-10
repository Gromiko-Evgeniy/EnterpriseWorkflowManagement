﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementService.Application.CQRS.ProjectTaskCommands;
using ProjectManagementService.Application.CQRS.ProjectTaskQueries;
using ProjectManagementService.Application.ProjectTaskDTOs;
using ProjectManagementService.Domain.Entities;

namespace ProjectManagementService.API.Controllers;
[Route("psroject-tasks")]
[ApiController]
public class ProjectTasksController : ControllerBase
{
    private readonly IMediator mediator;

    public ProjectTasksController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    //[Authorize(Roles = "DepartmentHead")]
    public async Task<IActionResult> GetAllAsync()
    {
        var tasks = await mediator.Send(new GetAllProjectTasksQuery());
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    //[Authorize(Roles = "DepartmentHead")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
    {
        var task = await mediator.Send(new GetProjectTaskByIdQuery(id));
        return Ok(task);
    }

    [HttpGet("projects/{id}")]
    //[Authorize(Roles = "DepartmentHead")]
    public async Task<IActionResult> GetByProjectIdAsync([FromRoute(Name = "id")] string projectId)
    {
        var tasks = await mediator.Send(new GetProjectTasksByProjectIdQuery(projectId));
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
        string id = await mediator.Send(new AddProjectTaskCommand(taskDTO)); 

        return Ok(id);
    }

    [HttpPut("cancel/{id}")]
    //[Authorize(Roles = "Customer")]
    public async Task<IActionResult> CancelAsync([FromRoute] string id, string customerId)// remove customerId from parameters
    {
        //Customer id will be extracted from JWT
        await mediator.Send(new CancelProjectTaskByIdCommand(id, customerId));
        return Ok();
    }

    [HttpPut("set-approvable/{id}")]
    //[Authorize(Roles = "Worker,ProjectLeader")]
    public async Task<IActionResult> MarkAsReadyToApproveAsync([FromRoute] string id, string workerId)// remove workerId from parameters
    {
        //Worker id will be extracted from JWT

        await mediator.Send(new MarkProjectTaskAsReadyToApproveCommand(id, workerId));
        return Ok();
    }

    [HttpPut("aprove/{id}")]
    //[Authorize(Roles = "ProjectLeader")]
    public async Task<IActionResult> MarkAsApproved([FromRoute] string id, string projectLeaderId) // remove projectLeaderId from parameters
    {
        //ProjectLeader id will be extracted from JWT
        await mediator.Send(new MarkProjectTaskAsApprovedCommand(id, projectLeaderId));
        return Ok();
    }

    [HttpPut("start-current")]
    //[Authorize(Roles = "Worker")]
    public async Task<IActionResult> StartWorkingOnTask(string workerId)// remove workerId from parameters
    {
        await mediator.Send(new StartWorkingOnTaskCommand(workerId));
        return Ok();
    }

    [HttpPut("finish-current")]
    //[Authorize(Roles = "Worker")]
    public async Task<IActionResult> FinishWorkingOnTask(string workerId)// remove workerId from parameters
    {
        //Worker id will be extracted from JWT

        await mediator.Send(new FinishWorkingOnTaskCommand(workerId));

        return Ok();
    }
}
