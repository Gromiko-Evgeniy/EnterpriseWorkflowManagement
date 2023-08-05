using EnterpriseWorkflowManagement.HiringService.Domain.DTOs.CandidateDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseWorkflowManagement.HiringService.API.Controllers
{
    [Route("сandidates")]
    [ApiController]
    public class CandidatesController : ControllerBase
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
        //[Authorize(Roles = "Candidate")]
        public async Task<IActionResult> GetCurrentAsync()
        {
            //id will be extracted from JWT
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddCandidateDTO candidate)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "Candidate")]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, UpdateCandidateDTO candidate)
        {
            //candidate id will be extracted from JWT
            return Ok();
        }

    }
}
