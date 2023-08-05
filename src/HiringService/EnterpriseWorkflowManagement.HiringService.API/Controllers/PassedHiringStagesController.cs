using Microsoft.AspNetCore.Mvc;

namespace EnterpriseWorkflowManagement.HiringService.API.Controllers
{
    [Route("passed-hiring-stages")]
    [ApiController]
    public class PassedHiringStagesController : ControllerBase
    {
        [HttpGet]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok();
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetByIdAsync(int id) 
        { 
            return Ok();
        }

        [HttpGet("current")]
        //[Authorize(Roles = "ProjectLeader,Worker")]
        public async Task<IActionResult> GetCurrentAsync()
        {
            //IntervierId will be extracted from JWT
            return Ok();
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "ProjectLeader,Worker")]
        public async Task<IActionResult> MarkAsPassedSuccessfullyAsync(int id)
        {
            return Ok();
        }

    }
}
