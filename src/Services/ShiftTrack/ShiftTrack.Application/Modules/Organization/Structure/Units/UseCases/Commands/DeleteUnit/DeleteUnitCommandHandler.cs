using ShiftTrack.Application.Modules.Organization.Structure.Units.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Commands.DeleteUnit;

public class DeleteUnitCommandHandler(
    IUnitService unitService) : IRequestHandler<DeleteUnitCommand>
{
    public async Task Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
    {
        await unitService.Delete(request.Id, cancellationToken);
    }
}