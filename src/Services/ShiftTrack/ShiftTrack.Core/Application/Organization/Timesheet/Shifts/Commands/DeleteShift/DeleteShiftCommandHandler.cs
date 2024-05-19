using Kernel.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShiftTrack.Core.Application.Data.Common.Interfaces;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.DeleteShift
{
    public class DeleteShiftCommandHandler : IRequestHandler<DeleteShiftCommand>
    {
        private readonly IApplicationDbContext _applicationDbContext;

        public DeleteShiftCommandHandler(
            IApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<Unit> Handle(DeleteShiftCommand request, CancellationToken cancellationToken)
        {
            var shift = await _applicationDbContext.Shifts
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (shift == null)
            {
                throw new EntityNotFoundException(typeof(Shift), request.Id);
            }

            _applicationDbContext.Shifts.Remove(shift);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
