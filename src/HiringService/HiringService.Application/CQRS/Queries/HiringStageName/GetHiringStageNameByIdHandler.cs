using HiringService.Application.Abstractions;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.StageNameQueries;

public class GetHiringStageNameByIdHandler : IRequestHandler<GetHiringStageNameByIdQuery, HiringStageName>
{
    private readonly IHiringStageNameRepository names;

    public GetHiringStageNameByIdHandler(IHiringStageNameRepository repository)
    {
        names = repository;
    }

    public async Task<HiringStageName> Handle(GetHiringStageNameByIdQuery request, CancellationToken cancellationToken)
    {
        return await names.GetByIdAsync(request.Id);
    }
}
