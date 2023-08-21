using IdentityService.Application.Abstractions.ServiceAbstractions.TokenServices;
using IdentityService.Application.DTOs;
using IdentityService.Application.ServiceAbstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseWorkflowManagement.IdentityService.API.Controllers;

[Route("workers")]
[ApiController]
public class WorkersController : ControllerBase
{
    private readonly IWorkerService _workerService;
    private readonly IWorkerTokenService _tokenService;

    public WorkersController(IWorkerService workerService, IWorkerTokenService tokenService)
    {
        _workerService = workerService;
        _tokenService = tokenService;
    }

    [HttpGet]
    [Authorize(Roles = "DepartmentHead")]
    public async Task<IActionResult> GetAllAsync()
    {
        var workerDTOs = await _workerService.GetAllAsync();

        return Ok(workerDTOs);
    }

    [HttpGet("{email:regex(^\\S+@\\S+\\.\\S+$)}")]
    [Authorize(Roles = "DepartmentHead")]
    public async Task<IActionResult> GetByEmailAsync([FromRoute] string email)
    {
        var workerDTO = await _workerService.GetByEmailAsync(email);

        return Ok(workerDTO);
    }

    [HttpGet("current")]
    [Authorize]
    public async Task<IActionResult> GetCurrentAsync(string email) // remove email
    {
        //email will be extracted from JWT

        var workerDTOs = await _workerService.GetAllAsync();

        return Ok(workerDTOs);
    }

    [HttpPost("log-in")]
    public async Task<IActionResult> LogInAsync(LogInData data)
    {
        var token = await _tokenService.GetTokenAsync(data);

        return Ok(token);
    }

    [HttpPut("new-name")]
    [Authorize(Roles = "Worker")]
    public async Task<IActionResult> UpadateNameAsync(string name, string email) // remove email
    {
        //email will be extracted from JWT

        await _workerService.UpdateNameAsync(email, name);

        return NoContent();        
    }

    [HttpPut("promote/{email}")]
    [Authorize(Roles = "DepartmentHead")]
    public async Task<IActionResult> PromoteAsync([FromRoute] string email)
    {
        await _workerService.PromoteAsync(email);

        return NoContent();
    }

    [HttpPut("demote/{email}")]
    [Authorize(Roles = "DepartmentHead")]
    public async Task<IActionResult> DemoteAsync([FromRoute] string email)
    {
        await _workerService.DemoteAsync(email);

        return NoContent();
    }

    [HttpDelete("dismiss/{email}")]
    [Authorize(Roles = "DepartmentHead")]
    public async Task<IActionResult> DismissWorkerAsync([FromRoute] string email)
    {
        await _workerService.DismissAsync(email);

        return NoContent();
    }

    [HttpDelete("quit")]
    [Authorize(Roles = "Worker")]
    public async Task<IActionResult> QuitAsync(string password, string email) // remove email
    {
        //id will be extracted from JWT
        //передать таску другому grpc

        await _workerService.QuitAsync(email, password);

        return NoContent();
    }
}
