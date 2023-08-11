using HiringService.Application.CQRS.HiringStageCommands;
using HiringService.Application.CQRS.StageQueries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HiringService.API.Controllers
{
    [Route("passed-hiring-stages")]
    [ApiController]
    public class HiringStagesController : ControllerBase
    {
        private readonly IMediator mediator;

        public HiringStagesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetAllAsync()
        {
            var stages = await mediator.Send(new GetHiringStagesQuery());

            return Ok(stages);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetByIdAsync(int id) 
        {
            var stage = await mediator.Send(new GetHiringStageByIdQuery(id));

            return Ok(stage);
        }

        [HttpGet("current")]
        //[Authorize(Roles = "ProjectLeader,Worker")]
        public async Task<IActionResult> GetCurrentAsync(int intervierId) // remove intervierId from parameters
        {
            //IntervierId will be extracted from JWT
            var stages = await mediator.Send(new GetHiringStageByIntervierIdQuery(intervierId));

            return Ok(stages);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "ProjectLeader,Worker")]
        public async Task<IActionResult> MarkAsPassedSuccessfullyAsync(int intervierId, int id) // remove intervierId
        {
            await mediator.Send(new MarkStageAsPassedSuccessfullyCommand(intervierId, id));

            return Ok();
        }
    }
}
