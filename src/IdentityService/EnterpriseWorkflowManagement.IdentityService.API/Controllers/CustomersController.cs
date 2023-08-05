using EnterpriseWorkflowManagement.IdentityService.Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseWorkflowManagement.IdentityService.API.Controllers
{
    [Route("customers")]
    [ApiController]
    public class CustomersController : ControllerBase
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
}
