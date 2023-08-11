using HiringService.Application.CQRS.StageNameCommands;
using HiringService.Application.CQRS.StageNameQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

namespace HiringService.API.Controllers
{
    [Route("hiring-stages")]
    [ApiController]
    public class HiringStageNamesController : ControllerBase
    {
        private readonly IMediator mediator;

        public HiringStageNamesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetAllAsync()
        {
            var stageNames = await mediator.Send(new GetHiringStageNamesQuery());

            return Ok(stageNames);
        }

        [HttpGet("{id:int}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var stageName = await mediator.Send(new GetHiringStageNameByIdQuery(id));

            return Ok(stageName);
        }

        [HttpGet("{name}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string name)
        {
            var stageName = await mediator.Send(new GetHiringStageNameByNameQuery(name));

            return Ok(stageName);
        }

        [HttpPost]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> AddAsync(string name)
        {
            var id = await mediator.Send(new AddStageNameCommand(name));

            return Ok(id);
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> RemoveAsync([FromRoute] int id)
        {
            await mediator.Send(new RemoveStageNameCommand(id));

            return Ok();
        }
    }
}
