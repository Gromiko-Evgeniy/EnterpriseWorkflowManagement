using IdentityService.Application.Abstractions.ServiceAbstractions.TokenServices;
using IdentityService.Application.DTOs;
using IdentityService.Application.DTOs.CandidateDTO;
using IdentityService.Application.ServiceAbstractions;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[Route("candidates")]
[ApiController]
public class CandidatesController : ControllerBase
{
    private readonly ICandidateService _candidateService;
    private readonly ICandidateTokenService _tokenService;
    public CandidatesController(ICandidateService candidateService, ICandidateTokenService tokenService)
    {
        _candidateService = candidateService;
        _tokenService = tokenService;
    }

    [HttpPost("log-in")]
    public async Task<IActionResult> LogIn(LogInData data)
    {
        var token = await _tokenService.GetTokenAsync(data);

        return Ok(token);
    }

    [HttpPost]
    public async Task<IActionResult> Registration(AddCandidateDTO candidateDTO)
    {
        var data = await _candidateService.AddAsync(candidateDTO);

        var token = await _tokenService.GetTokenAsync(data);

        return Ok(token);
    }
}
