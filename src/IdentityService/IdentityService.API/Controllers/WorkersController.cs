using IdentityService.Application.TokenAbstractions;
using IdentityService.Application.DTOs;
using IdentityService.Application.ServiceAbstractions;
using IdentityService.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace EnterpriseWorkflowManagement.IdentityService.API.Controllers;

[Route("workers")]
[ApiController]
public class WorkersController : ControllerBase
{
    private readonly IWorkerService _workerService;
    private readonly IWorkerTokenService _tokenService;
    private readonly IJWTExtractorService _JWTExtractorService;
    private const string _depHeadRole = nameof(ApplicationRole.DepartmentHead);
    private const string _workerRole = nameof(ApplicationRole.Worker);

    public WorkersController(IWorkerService workerService,
        IWorkerTokenService tokenService, IJWTExtractorService JWTExtractorService)
    {
        _workerService = workerService;
        _tokenService = tokenService;
        _JWTExtractorService = JWTExtractorService;
    }

    [HttpGet]
    [Authorize(Roles = _depHeadRole)]
    public async Task<IActionResult> GetAllAsync()
    {
        var workerDTOs = await _workerService.GetAllAsync();

        return Ok(workerDTOs);
    }

    [HttpGet("{email:regex(^\\S+@\\S+\\.\\S+$)}")]
    [Authorize(Roles = _depHeadRole)]
    public async Task<IActionResult> GetByEmailAsync([FromRoute] string email)
    {
        var workerDTO = await _workerService.GetByEmailAsync(email);

        return Ok(workerDTO);
    }

    [HttpGet("current")]
    [Authorize]
    public async Task<IActionResult> GetCurrentAsync()
    {
        var email = _JWTExtractorService.ExtractClaim(HttpContext.Request, "email");

        var workerDTO = await _workerService.GetByEmailAsync(email);

        return Ok(workerDTO);
    }

    [HttpPost("log-in")]
    public async Task<IActionResult> LogInAsync(LogInData data)
    {
        //try
        //{
            var token = await _tokenService.GetTokenAsync(data);
            return Ok(token);


        //}
        //catch (Exception ex) {
        //    return Ok(ex);

        //}

    }

    [HttpPut("new-name")]
    [Authorize(Roles = _workerRole)]
    public async Task<IActionResult> UpdateNameAsync([FromBody] string name)
    {
        var email = _JWTExtractorService.ExtractClaim(HttpContext.Request, "email");

        await _workerService.UpdateNameAsync(email, name);

        return NoContent();        
    }

    [HttpPut("promote/{email}")]
    [Authorize(Roles = _depHeadRole)]
    public async Task<IActionResult> PromoteAsync([FromRoute] string email)
    {
        await _workerService.PromoteAsync(email);

        return NoContent();
    }

    [HttpPut("demote/{email}")]
    [Authorize(Roles = _depHeadRole)]
    public async Task<IActionResult> DemoteAsync([FromRoute] string email)
    {
        await _workerService.DemoteAsync(email);

        return NoContent();
    }

    [HttpDelete("dismiss/{email}")]
    [Authorize(Roles = _depHeadRole)]
    public async Task<IActionResult> DismissWorkerAsync([FromRoute] string email)
    {
        await _workerService.DismissAsync(email);

        return NoContent();
    }

    [HttpDelete("quit")]
    [Authorize(Roles = _workerRole)]
    public async Task<IActionResult> QuitAsync(string password)
    {
        var email = _JWTExtractorService.ExtractClaim(HttpContext.Request, "email");
        
        await _workerService.QuitAsync(email, password);

        return NoContent();
    }
}

//передать таску другому grpc