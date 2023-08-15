using HiringService.Application.CQRS.CandidateCommands;
using HiringService.Application.CQRS.CandidateQueries;
using HiringService.Domain.DTOs.CandidateDTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HiringService.API.Controllers
{
    [Route("сandidates")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CandidatesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetAllAsync()
        {
            var candidates = await _mediator.Send(new GetCandidatesQuery());

            return Ok(candidates);
        }

        [HttpGet("{id}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id) 
        {
            var candidate = await _mediator.Send(new GetCandidateByIdQuery(id));

            return Ok(candidate);
        }

        [HttpGet("{email:regex(^\\S+@\\S+\\.\\S+$)}")]
        //[Authorize(Roles = "DepartmentHead")]
        public async Task<IActionResult> GetByEmailAsync([FromRoute] string email)
        {
            var candidate = await _mediator.Send(new GetCandidateByEmailQuery(email));

            return Ok(candidate);
        }

        [HttpGet("current")]
        //[Authorize(Roles = "Candidate")]
        public async Task<IActionResult> GetCurrentAsync(int candidateId) //remove candidateId
        {
            var candidate = await _mediator.Send(new GetCandidateByIdQuery(candidateId));

            return Ok(candidate);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(AddCandidateDTO candidate)
        {
            var id = await _mediator.Send(new AddCandidateCommand(candidate));
            
            return Ok(id);
        }

        [HttpPut("new-name")]
        //[Authorize(Roles = "Candidate")]
        public async Task<IActionResult> UpdateNameAsync(int candidateId, string name) //remove candidateId
        {
            await _mediator.Send(new UpdateCandidateNameCommand(candidateId, name));
            return NoContent();
        }

        [HttpPut("new-cv")]
        //[Authorize(Roles = "Candidate")]
        public async Task<IActionResult> UpdateCVAsync(int candidateId, string CV) //remove candidateId
        {
            await _mediator.Send(new UpdateCandidateCVCommand(candidateId, CV));
            return NoContent();
        }
    }
}
