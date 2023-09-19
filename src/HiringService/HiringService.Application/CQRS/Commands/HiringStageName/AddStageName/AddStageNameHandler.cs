using HiringService.Application.Abstractions.RepositoryAbstractions;
using HiringService.Application.Exceptions.HiringStageName;
using HiringService.Domain.Entities;
using MediatR;
using System.Security.Cryptography.X509Certificates;

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
            await RemovePossibleIndexErrorsAsync(stageNames);

            newStageName.Index = stageNames.Count;
        }

        newStageName = _nameRepository.Add(newStageName);
        await _nameRepository.SaveChangesAsync();

        return newStageName.Id;
    }

    private async Task RemovePossibleIndexErrorsAsync(List<HiringStageName> stageNames)
    {
        stageNames.Sort((x, y) => x.Index.CompareTo(y.Index));

        for (int i = 0; i < stageNames.Count; i++)
        {
            if (stageNames[i].Index != i)
            {
                stageNames[i].Index = i;
                _nameRepository.Update(stageNames[i]);
                await _nameRepository.SaveChangesAsync();
            }
        }
    }
}

