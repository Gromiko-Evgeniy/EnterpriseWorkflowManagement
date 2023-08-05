using Microsoft.AspNetCore.Mvc;

namespace EnterpriseWorkflowManagement.HiringService.API.Controllers
{
    [Route("hiring-stages")]
    [ApiController]
    public class HiringStagesController : ControllerBase
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


        [HttpPost]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> AddAsync(string name)
        {
            return Ok();
        }


        [HttpDelete("{id}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id)
        {
            return Ok();
        }
    }
}
