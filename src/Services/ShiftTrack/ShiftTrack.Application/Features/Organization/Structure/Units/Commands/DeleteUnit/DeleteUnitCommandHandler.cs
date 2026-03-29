using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Structure.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Organization.Structure.Units.Commands.DeleteUnit;

public class DeleteUnitCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<DeleteUnitCommand>
{
    public async Task Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await dbContext.Units
            .Include(x => x.Departments)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (unit == null)
            throw new EntityNotFoundException(typeof(Unit), request.Id);

        dbContext.Departments.RemoveRange(unit.Departments);
        dbContext.Units.Remove(unit);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}