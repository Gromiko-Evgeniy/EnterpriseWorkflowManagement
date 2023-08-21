﻿using IdentityService.Application.Abstractions.ServiceAbstractions.TokenServices;
using IdentityService.Application.DTOs;
using IdentityService.Application.DTOs.CustomerDTOs;
using IdentityService.Application.ServiceAbstractions;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.API.Controllers;

[Route("customers")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly ICustomerTokenService _tokenService;


    public CustomersController(ICustomerService customerService, ICustomerTokenService tokenService)
    {
        _customerService = customerService;
        _tokenService = tokenService;
    }

    [HttpPost("log-in")]
    public async Task<IActionResult> LogIn(LogInData data)
    {
        var token = await _tokenService.GetTokenAsync(data);

        return Ok(token);
    }

    [HttpPost]
    public async Task<IActionResult> Registration(AddCustomerDTO customerDTO)
    {
        var data = await _customerService.AddAsync(customerDTO);

        var token = await _tokenService.GetTokenAsync(data);

        return Ok(token);
    }
}
