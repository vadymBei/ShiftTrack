using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.CreateUnit;

public class CreateUnitCommandHandler(
    IMapper mapper,
    IUnitRepository unitRepository)
    : IRequestHandler<CreateUnitCommand, UnitVm>
{
    public async Task<UnitVm> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await unitRepository.Create(request.Data, cancellationToken);

        return mapper.Map<UnitVm>(unit);
    }
}