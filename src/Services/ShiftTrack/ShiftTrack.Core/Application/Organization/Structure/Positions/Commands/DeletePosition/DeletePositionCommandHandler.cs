using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Domain.Organization.Structure.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.DeletePosition;

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