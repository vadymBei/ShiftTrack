using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Kernel.CQRS.Interfaces;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Shifts.Commands.DeleteShift;

public class DeleteShiftCommandHandler(
    IApplicationDbContext applicationDbContext) : IRequestHandler<DeleteShiftCommand>
{
    public async Task Handle(DeleteShiftCommand request, CancellationToken cancellationToken)
    {
        var shift = await applicationDbContext.Shifts
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (shift == null)
        {
            throw new EntityNotFoundException(typeof(Shift), request.Id);
        }

        applicationDbContext.Shifts.Remove(shift);
        await applicationDbContext.SaveChangesAsync(cancellationToken);
    }
}