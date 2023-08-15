using HiringService.Application.Abstractions;
using HiringService.Application.Exceptions.HiringStageName;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.StageNameQueries;

public class GetHiringStageNameByNameHandler : IRequestHandler<GetHiringStageNameByNameQuery, HiringStageName>
{
    private readonly IHiringStageNameRepository _nameRepository;

    public GetHiringStageNameByNameHandler(IHiringStageNameRepository nameRepository)
    {
        _nameRepository = nameRepository;
    }

    public async Task<HiringStageName> Handle(GetHiringStageNameByNameQuery request, CancellationToken cancellationToken)
    {
        var stageName = await _nameRepository.GetByNameAsync(request.Name);

        if (stageName == null) throw new NoStageNameWithSuchNameException();

        return stageName;
    }
}

