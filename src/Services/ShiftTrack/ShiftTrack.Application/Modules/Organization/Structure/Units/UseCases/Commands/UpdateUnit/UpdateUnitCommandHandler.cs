using AutoMapper;
using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.UpdateUnit;

public class UpdateUnitCommandHandler(
    IMapper mapper,
    IUnitRepository unitRepository)
    : IRequestHandler<UpdateUnitCommand, UnitVm>
{
    public async Task<UnitVm> Handle(UpdateUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await unitRepository.Update(request.Data, cancellationToken);

        return mapper.Map<UnitVm>(unit);
    }
}