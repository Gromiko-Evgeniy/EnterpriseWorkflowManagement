using HiringService.Application.CQRS.StageNameCommands;
using HiringService.Application.CQRS.StageNameQueries;
using HiringService.Application.DTOs.StageNameDTOs;
using HiringService.Domain.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HiringService.API.Controllers
{
    [Route("hiring-stage-names")]
    [ApiController]
    //[Authorize(Roles = _depHeadRole)]
    public class HiringStageNamesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private const string _depHeadRole = nameof(ApplicationRole.DepartmentHead);

        public HiringStageNamesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var stageNames = await _mediator.Send(new GetHiringStageNamesQuery());

            return Ok(stageNames);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var stageName = await _mediator.Send(new GetHiringStageNameByIdQuery(id));

            return Ok(stageName);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByNamesync([FromRoute] string name)
        {
            var stageName = await _mediator.Send(new GetHiringStageNameByNameQuery(name));

            return Ok(stageName);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] AddStageNameDTO hiringStageDTO)
        {
            var id = await _mediator.Send(new AddStageNameCommand(hiringStageDTO));

            return Ok(id);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveAsync([FromRoute] int id)
        {
            await _mediator.Send(new RemoveStageNameCommand(id));

            return NoContent();
        }
    }
}
