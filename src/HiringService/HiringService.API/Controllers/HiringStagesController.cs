using HiringService.Application.CQRS.HiringStageCommands;
using HiringService.Application.CQRS.HiringStageQueries;
using HiringService.Application.DTOs.HiringStageDTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HiringService.API.Controllers
{
    [Route("hiring-stages")]
    [ApiController]
    public class HiringStagesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HiringStagesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetAllAsync()
        {
            var stages = await _mediator.Send(new GetHiringStagesQuery());

            return Ok(stages);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var stage = await _mediator.Send(new GetHiringStageByIdQuery(id));

            return Ok(stage);
        }

        [HttpGet("current")]
        //[Authorize(Roles = "ProjectLeader,Worker")]
        public async Task<IActionResult> GetCurrentAsync(int intervierId) // remove intervierId from parameters
        {
            //IntervierId will be extracted from JWT
            var stages = await _mediator.Send(new GetHiringStageByIntervierIdQuery(intervierId));

            return Ok(stages);
        }

        [HttpPost]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> MarkAsPassedSuccessfullyAsync(AddHiringStageDTO hiringStageDTO) // remove intervierId
        {
            var id = await _mediator.Send(new AddHiringStageCommand(hiringStageDTO));

            return Ok(id);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "DepartmentHead,ProjectLeader,Worker")]
        public async Task<IActionResult> MarkAsPassedSuccessfullyAsync(int intervierId, int id) // remove intervierId
        {
            await _mediator.Send(new MarkStageAsPassedSuccessfullyCommand(intervierId, id));

            return NoContent();
        }
    }
}
