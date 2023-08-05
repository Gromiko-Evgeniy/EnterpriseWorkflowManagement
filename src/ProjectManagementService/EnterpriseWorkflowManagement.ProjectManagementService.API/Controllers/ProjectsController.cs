using EnterpriseWorkflowManagement.ProjectManagementService.Domain.DTOs.ProjectDTOs;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseWorkflowManagement.ProjectManagementService.API.Controllers
{
    [Route("projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
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

        [HttpGet("current-customer-projects")]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCustomerProjectsAsync()
        {
            //Customer id will be extracted from JWT
            return Ok();
        }

        [HttpGet("current-customer-projects/{id}")] 
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCustomerProjectByIdAsync([FromRoute] int id)
        {
            return Ok();
        }

        [HttpGet("current")]
        //[Authorize(Roles = "ProjectLeader")]
        public async Task<IActionResult> GetProjectLeaderProject()
        {
            // ProjectLeader id will be extracted from JWT
            return Ok();
        }

        [HttpPost]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddAsync(AddProjectDTO candidate)
        {
            //Customer id will be extracted from JWT
            return Ok();
        }
    }
}