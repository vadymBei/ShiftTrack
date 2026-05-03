using ShiftTrack.Application.Modules.Organization.Structure.Positions.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.DeletePosition;

public class DeletePositionCommandHandler(
    IPositionRepository positionRepository)
    : IRequestHandler<DeletePositionCommand>
{
    public async Task Handle(DeletePositionCommand request, CancellationToken cancellationToken)
    {
       await positionRepository.Delete(request.Id, cancellationToken);
    }
}