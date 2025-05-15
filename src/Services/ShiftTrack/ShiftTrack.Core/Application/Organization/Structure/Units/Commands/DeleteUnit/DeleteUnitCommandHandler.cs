using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

using UnitEntity = ShiftTrack.Core.Domain.Organization.Structure.Entities.Unit;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Commands.DeleteUnit;

public class DeleteUnitCommandHandler(
    IApplicationDbContext dbContext) : IRequestHandler<DeleteUnitCommand>
{
    public async Task Handle(DeleteUnitCommand request, CancellationToken cancellationToken)
    {
        var unit = await dbContext.Units
            .Include(x => x.Departments)
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (unit == null)
            throw new EntityNotFoundException(typeof(UnitEntity), request.Id);

        dbContext.Departments.RemoveRange(unit.Departments);
        dbContext.Units.Remove(unit);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}