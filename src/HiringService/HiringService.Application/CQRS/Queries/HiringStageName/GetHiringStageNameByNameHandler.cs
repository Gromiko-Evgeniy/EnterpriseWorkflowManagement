using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.StageNameQueries;

public class GetHiringStageNameByNameHandler : IRequestHandler<GetHiringStageNameByNameQuery, HiringStageName>
{
    private readonly IHiringStageNameRepository names;

    public GetHiringStageNameByNameHandler(IHiringStageNameRepository repository)
    {
        names = repository;
    }

    public async Task<HiringStageName> Handle(GetHiringStageNameByNameQuery request, CancellationToken cancellationToken)
    {
        return await names.GetByNameAsync(request.Name);
    }
}

