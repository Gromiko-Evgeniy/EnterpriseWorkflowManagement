using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Exceptions.HiringStageName;
using HiringService.Domain.Entities;
using MediatR;

namespace HiringService.Application.CQRS.StageNameCommands;

public class AddStageNameHandler : IRequestHandler<AddStageNameCommand, int>
{
    private readonly IHiringStageNameRepository _nameRepository;

    public AddStageNameHandler(IHiringStageNameRepository nameRepository)
    {
        _nameRepository = nameRepository;
    }

    public async Task<int> Handle(AddStageNameCommand request, CancellationToken cancellationToken)
    {
        var addNameDTO = request.StageNameDTO;

        var OldStageName = await _nameRepository.GetByNameAsync(addNameDTO.Name);
        if (OldStageName is not null) throw new StageNameAlreadyExistsException();

        var newStageName = new HiringStageName() { Name = addNameDTO.Name, Index = addNameDTO.Index };

        var stageNames = await _nameRepository.GetAllAsync(); // shifting the indices of all subsequent elements in the list
        if (stageNames.Any(n => n.Index == addNameDTO.Index))
        {
            foreach (var stageName in stageNames.Where(n => n.Index >= addNameDTO.Index))
            {
                stageName.Index += 1;
                _nameRepository.Update(stageName);
            }
        }
        else
        {
            //проеверить: возможно наибольший индекс меньше количества имен
            //(0,1,__3,4  count<5) -> не столь важно
            //(__1,2,3,4  count<5) -> индекс дублируется, важно
            //но такое возможно лишь при тупом удалении данных прямиком из базы

            newStageName.Index = stageNames.Count;
        }

        newStageName = _nameRepository.Add(newStageName);
        await _nameRepository.SaveChangesAsync();

        return newStageName.Id;
    }
}

