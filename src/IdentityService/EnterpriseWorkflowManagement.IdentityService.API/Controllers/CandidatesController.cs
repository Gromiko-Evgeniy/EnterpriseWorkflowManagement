using EnterpriseWorkflowManagement.IdentityService.Domain.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseWorkflowManagement.IdentityService.API.Controllers;
[Route("candidates")]
[ApiController]
public class CandidatesController : ControllerBase
{
    [HttpPost("log-in")]
    public async Task<IActionResult> LogIn(LogInData data)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Registration(LogInData data)
    {
        return Ok();
    }
}
