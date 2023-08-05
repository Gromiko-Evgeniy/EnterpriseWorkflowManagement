using EnterpriseWorkflowManagement.ProjectManagementService.Domain.DTOs.ProjectTaskDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseWorkflowManagement.ProjectManagementService.API.Controllers;
[Route("psroject-tasks")]
[ApiController]
public class ProjectTasksController : ControllerBase
{
    [HttpGet]
    //[Authorize(Roles = "DepartmentHead")]
    public async Task<IActionResult> GetAllAsync()
    {
        return Ok();
    }

    [HttpGet("{id}")]
    //[Authorize(Roles = "DepartmentHead")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
    {
        return Ok();
    }

    [HttpGet("projects/{id}")]
    //[Authorize(Roles = "DepartmentHead")]
    public async Task<IActionResult> GetByProjectIdAsync([FromRoute] int id)
    {
        return Ok(id);
    }

    [HttpGet("current")] 
    //[Authorize(Roles = "Worker,ProjectLeader")]
    public async Task<IActionResult> GetCurrentAsync()
    {
        //Worker id will be extracted from JWT
        return Ok();
    }

    [HttpPost]
    //[Authorize(Roles = "Customer")]
    public async Task<IActionResult> AddAsync(AddProjectTaskDTO task)
    {
        //Customer id will be extracted from JWT
        return Ok();
    }

    [HttpPut("cancel/{id}")]
    //[Authorize(Roles = "Customer")]
    public async Task<IActionResult> CancelAsync([FromRoute] int id)
    {
        //Customer id will be extracted from JWT
        return Ok();
    }

    [HttpPut("set-approvable/{id}")]
    //[Authorize(Roles = "Worker,ProjectLeader")]
    public async Task<IActionResult> MarkAsReadyToApproveAsync([FromRoute] int id)
    {
        //Worker id will be extracted from JWT
        return Ok();
    }

    [HttpPut("aprove/{id}")]
    //[Authorize(Roles = "ProjectLeader")]
    public async Task<IActionResult> MarkAsApproved([FromRoute] int id)
    {
        //Worker id will be extracted from JWT
        return Ok();
    }

}
