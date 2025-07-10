using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Organization.Structure.Positions.Commands.DeletePosition;

public class DeletePositionCommandHandler(IApplicationDbContext applicationDbContext)
    : IRequestHandler<DeletePositionCommand>
{
    public async Task Handle(DeletePositionCommand request, CancellationToken cancellationToken)
    {
        var position = await applicationDbContext.Positions
            .FindAsync(request.Id);

        if (position is null)
        {
            throw new EntityNotFoundException(typeof(Position), request.Id);
        }

        applicationDbContext.Positions.Remove(position);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}