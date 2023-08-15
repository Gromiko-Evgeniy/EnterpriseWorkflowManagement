using HiringService.Application.CQRS.StageNameCommands;
using HiringService.Application.CQRS.StageNameQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HiringService.API.Controllers
{
    [Route("hiring-stage-names")]
    [ApiController]
    public class HiringStageNamesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HiringStageNamesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetAllAsync()
        {
            var stageNames = await _mediator.Send(new GetHiringStageNamesQuery());

            return Ok(stageNames);
        }

        [HttpGet("{id:int}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var stageName = await _mediator.Send(new GetHiringStageNameByIdQuery(id));

            return Ok(stageName);
        }

        [HttpGet("{name}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string name)
        {
            var stageName = await _mediator.Send(new GetHiringStageNameByNameQuery(name));

            return Ok(stageName);
        }

        [HttpPost]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> AddAsync(string name)
        {
            var id = await _mediator.Send(new AddStageNameCommand(name));

            return Ok(id);
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> RemoveAsync([FromRoute] int id)
        {
            await _mediator.Send(new RemoveStageNameCommand(id));

            return NoContent();
        }
    }
}
