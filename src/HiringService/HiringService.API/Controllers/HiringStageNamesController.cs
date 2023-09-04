using HiringService.Application.CQRS.HiringStageCommands;
using HiringService.Application.CQRS.StageNameCommands;
using HiringService.Application.CQRS.StageNameQueries;
using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Domain.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiringService.API.Controllers
{
    [Route("hiring-stage-names")]
    [ApiController]
    public class HiringStageNamesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private const string _depHeadRole = nameof(ApplicationRole.DepartmentHead);

        public HiringStageNamesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = _depHeadRole)]
        public async Task<IActionResult> GetAllAsync()
        {
            var stageNames = await _mediator.Send(new GetHiringStageNamesQuery());

            return Ok(stageNames);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = _depHeadRole)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var stageName = await _mediator.Send(new GetHiringStageNameByIdQuery(id));

            return Ok(stageName);
        }

        [HttpGet("{name}")]
        [Authorize(Roles = _depHeadRole)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] string name)
        {
            var stageName = await _mediator.Send(new GetHiringStageNameByNameQuery(name));

            return Ok(stageName);
        }

        [HttpPost]
        [Authorize(Roles = _depHeadRole)]
        public async Task<IActionResult> AddAsync([FromBody] AddHiringStageDTO hiringStageDTO)
        {
            var id = await _mediator.Send(new AddHiringStageCommand(hiringStageDTO));

            return Ok(id);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = _depHeadRole)]
        public async Task<IActionResult> RemoveAsync([FromRoute] int id)
        {
            var newJWT = await _mediator.Send(new RemoveStageNameCommand(id));

            if (newJWT is not null) return Ok(newJWT);

            return NoContent();
        }
    }
}
