using HiringService.Application.Abstractions;
using HiringService.Application.Exceptions.HiringStageName;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.StageNameQueries;


public class GetHiringStageNameByIdHandler : IRequestHandler<GetHiringStageNameByIdQuery, HiringStageName>
{
    private readonly IHiringStageNameRepository _nameRepository;

    public GetHiringStageNameByIdHandler(IHiringStageNameRepository nameRepository)
    {
        _nameRepository = nameRepository;
    }

    public async Task<HiringStageName> Handle(GetHiringStageNameByIdQuery request, CancellationToken cancellationToken)
    {

        var stageName = await _nameRepository.GetByIdAsync(request.Id);

        if (stageName == null) throw new NoStageNameWithSuchIdException();

        return stageName;
    }
}
