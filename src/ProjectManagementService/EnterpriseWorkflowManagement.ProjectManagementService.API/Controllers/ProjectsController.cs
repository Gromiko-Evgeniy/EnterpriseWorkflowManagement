using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementService.Application.CQRS.ProjectCommands;
using ProjectManagementService.Application.CQRS.ProjectQueries;
using ProjectManagementService.Application.ProjectDTOs;

namespace ProjectManagementService.API.Controllers
{
    [Route("projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProjectsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetAllAsync() 
        {
            var projects = await mediator.Send(new GetAllProjectsQuery());
            return Ok(projects);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
        {
            var project = await mediator.Send(new GetProjectByIdQuery(id));
            return Ok(project);
        }

        [HttpGet("current-customer-projects")]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetAllCustomerProjectsAsync(string customerId)// remove customerId from parameters
        {
            //Customer id will be extracted from JWT
            //var customerId = "";

            var project = await mediator.Send(new GetAllCustomerProjectsQuery(customerId));
            return Ok(project); 
        }

        [HttpGet("current-customer-projects/{id}")] 
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> GetCustomerProjectByIdAsync([FromRoute] string id, string customerId)// remove customerId from parameters
        {
            var project = await mediator.Send(new GetCustomerProjectByIdQuery(customerId, id));
            return Ok(project);
        }

        [HttpGet("current")]
        //[Authorize(Roles = "ProjectLeader")]
        public async Task<IActionResult> GetProjectLeaderProject(string projectLeaderId)// remove projectLeaderId from parameters
        {
            // ProjectLeader id will be extracted from JWT

            var project = await mediator.Send(new GetProjectLeaderProjectQuery(projectLeaderId));

            return Ok(project);
        }

        [HttpPost]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> AddAsync(AddProjectDTO addProjectDTO, string customerId)// remove customerId from parameters
        {
            //Customer id will be extracted from JWT
            string id = await mediator.Send(new AddProjectCommand(addProjectDTO, customerId));

            return Ok(id);
        }
    }
}