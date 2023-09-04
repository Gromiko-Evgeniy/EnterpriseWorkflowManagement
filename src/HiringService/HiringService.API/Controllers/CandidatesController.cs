using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Abstractions.ServiceAbstractions;
using HiringService.Application.CQRS.CandidateCommands;
using HiringService.Application.CQRS.CandidateQueries;
using HiringService.Application.DTOs.CandidateDTOs;
using HiringService.Domain.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HiringService.API.Controllers
{
    [Route("сandidates")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICandidateRepository _candidateRepository;
        private readonly IGRPCService _gRPCService;
        private readonly IJWTExtractorService _JWTExtractorService;
        private const string _depHeadRole = nameof(ApplicationRole.DepartmentHead);
        private const string _candidateRole = nameof(ApplicationRole.Candidate);

        public CandidatesController(IMediator mediator, ICandidateRepository candidateRepository,
            IGRPCService gRPCService, IJWTExtractorService JWTExtractorService)
        {
            _mediator = mediator;
            _candidateRepository = candidateRepository;
            _gRPCService = gRPCService;
            _JWTExtractorService = JWTExtractorService;
        }

        [HttpGet]
        [Authorize(Roles = _depHeadRole)]
        public async Task<IActionResult> GetAllAsync()
        {
            var candidates = await _mediator.Send(new GetCandidatesQuery());

            return Ok(candidates);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = _depHeadRole)]
        public async Task<IActionResult> GetByIdAsync([FromRoute] int id)
        {
            var candidate = await _mediator.Send(new GetCandidateByIdQuery(id));

            return Ok(candidate);
        }

        [HttpGet("{email:regex(^\\S+@\\S+\\.\\S+$)}")]
        [Authorize(Roles = _depHeadRole)]
        public async Task<IActionResult> GetByEmailAsync([FromRoute] string email)
        {
            var candidate = await _mediator.Send(new GetWorkerByEmailQuery(email));

            return Ok(candidate);
        }

        [HttpGet("current")]
        [Authorize(Roles = _candidateRole)]
        public async Task<IActionResult> GetCurrentAsync()
        {
            var email = _JWTExtractorService.ExtractClaim(HttpContext.Request, "email");

            var candidate = await _mediator.Send(new GetCandidateByEmailQuery(email));

            return Ok(candidate);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] AddCandidateDTO candidate)
        {
            var id = await _mediator.Send(new AddCandidateCommand(candidate));

            return Ok(id);
        }

        [HttpPut("new-name")]
        [Authorize(Roles = _candidateRole)]
        public async Task<IActionResult> UpdateNameAsync([FromBody] string name)
        {
            var email = _JWTExtractorService.ExtractClaim(HttpContext.Request, "email");

            var candidate = await _mediator.Send(new GetCandidateByEmailQuery(email));

            await _mediator.Send(new UpdateCandidateNameCommand(candidate.Id, name));

            return NoContent();
        }

        [HttpPut("new-cv")]
        [Authorize(Roles = _candidateRole)]
        public async Task<IActionResult> UpdateCVAsync([FromBody] string CV)
        {
            var email = _JWTExtractorService.ExtractClaim(HttpContext.Request, "email");

            var candidate = await _mediator.Send(new GetCandidateByEmailQuery(email));

            await _mediator.Send(new UpdateCandidateCVCommand(candidate.Id, CV));

            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int candidateId) //REMOVE_!!!____IT'S_FOR_TESTING
        {
            var candidate = await _candidateRepository.GetByIdAsync(candidateId);

            //_candidateRepository.Remove(candidate);
            //await _candidateRepository.SaveChangesAsync();

            var newJWT = await _gRPCService.DeleteCandidate(candidate.Email, candidate.Name);

            if (newJWT is not null) return Ok(newJWT);

            return NoContent();
        }
    }
}
