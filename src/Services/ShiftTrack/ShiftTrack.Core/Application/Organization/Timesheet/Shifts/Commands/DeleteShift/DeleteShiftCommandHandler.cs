using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.DeleteShift;

public class DeleteShiftCommandHandler(
    IApplicationDbContext applicationDbContext) : IRequestHandler<DeleteShiftCommand>
{
    public async Task<Unit> Handle(DeleteShiftCommand request, CancellationToken cancellationToken)
    {
        var shift = await applicationDbContext.Shifts
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (shift == null)
        {
            throw new EntityNotFoundException(typeof(Shift), request.Id);
        }

        applicationDbContext.Shifts.Remove(shift);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}