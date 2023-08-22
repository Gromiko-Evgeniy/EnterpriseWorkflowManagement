using IdentityService.Domain.DTOs;
using IdentityService.Domain.DTOs.WorkerDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseWorkflowManagement.IdentityService.API.Controllers
{
    [Route("workers")]
    [ApiController]
    public class WorkersController : ControllerBase
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

        [HttpGet("{email:regex(^\\S+@\\S+\\.\\S+$)}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetByEmailAsync([FromRoute] string email)
        {
            return Ok();
        }

        [HttpGet("current")]
        //[Authorize]
        public async Task<IActionResult> GetCurrentAsync()
        {
            //id will be extracted from JWT
            return Ok();
        }

        [HttpPost("log-in")]
        public async Task<IActionResult> LogInAsync(LogInData data)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Worker")]
        public async Task<IActionResult> UpadateAsync([FromRoute] int id, UpdateWorkerDTO data)
        {
            return Ok();
            //overwrite data in other databases
        }

        [HttpPut("promote/{id}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> PromoteAsync([FromRoute] int id)
        {
            return Ok();
        }

        [HttpPut("demote/{id}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> DemoteAsync([FromRoute] int id) 
        {
            return Ok();
        }

        [HttpDelete("dismiss/{id}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> DismissWorkerAsync([FromRoute] int id)
        {
            return Ok();
        }

        [HttpDelete("quit")]
        //[Authorize(Roles = "Worker")]
        public async Task<IActionResult> QuitAsync()
        {
            //id will be extracted from JWT
            //передать таску другому
            return Ok();
        }
    }
}
