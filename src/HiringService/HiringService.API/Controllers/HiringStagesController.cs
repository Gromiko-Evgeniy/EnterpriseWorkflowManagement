using HiringService.Application.Abstractions.ServiceAbstractions;
using HiringService.Application.CQRS.CandidateQueries;
using HiringService.Application.CQRS.HiringStageCommands;
using HiringService.Application.CQRS.HiringStageQueries;
using HiringService.Application.CQRS.WorkerQueries;
using HiringService.Application.DTOs.HiringStageDTOs;
using HiringService.Domain.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiringService.API.Controllers
{
    [Route("hiring-stages")]
    [ApiController]
    public class HiringStagesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IJWTExtractorService _JWTExtractorService;
        private const string _depHeadRole = nameof(ApplicationRole.DepartmentHead);
        private const string _workerRole = nameof(ApplicationRole.Worker);
        private const string _leaderRole = nameof(ApplicationRole.ProjectLeader);

        public HiringStagesController(IMediator mediator, IJWTExtractorService JWTExtractorService)
        {
            _mediator = mediator;
            _JWTExtractorService = JWTExtractorService;
        }

        [HttpGet]
        [Authorize(Roles = _depHeadRole)]
        public async Task<IActionResult> GetAllAsync()
        {
            var stages = await _mediator.Send(new GetHiringStagesQuery());

            return Ok(stages);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = _depHeadRole)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var stage = await _mediator.Send(new GetHiringStageByIdQuery(id));

            return Ok(stage);
        }

        [HttpGet("current")]
        [Authorize(Roles = $"{_leaderRole},{_workerRole}")]
        public async Task<IActionResult> GetCurrentAsync()
        {
            var email = _JWTExtractorService.ExtractClaim(HttpContext.Request, "email");

            var worker = await _mediator.Send(new GetWorkerByEmailQuery(email));

            var stages = await _mediator.Send(new GetHiringStagesByIntervierIdQuery(worker.Id));

            return Ok(stages);
        }

        [HttpPut("mark-as-success/{id}")]
        [Authorize(Roles = $"{_depHeadRole},{_leaderRole},{_workerRole}")]
        public async Task<IActionResult> MarkAsPassedSuccessfullyAsync([FromRoute] int id)
        {
            var email = _JWTExtractorService.ExtractClaim(HttpContext.Request, "email");

            var worker = await _mediator.Send(new GetWorkerByEmailQuery(email));

            var newJWT = await _mediator.Send(new MarkStageAsPassedSuccessfullyCommand(worker.Id, id));

            if (newJWT is not null) return Ok(newJWT);

            return NoContent();
        }
    }
}
