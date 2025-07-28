using Microsoft.EntityFrameworkCore;
using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Features.Booking.Common.Interfaces;
using ShiftTrack.Domain.Features.Booking.Vacations.Entities;
using ShiftTrack.Kernel.Exceptions;

namespace ShiftTrack.Application.Features.Booking.Common.Services;

public class VacationService(
    IApplicationDbContext applicationDbContext) : IVacationService
{
    public async Task<Vacation> GetById(object id, CancellationToken cancellationToken)
    {
        var vacation = await applicationDbContext.Vacations
            .AsNoTracking()
            .Include(x => x.Employee)
            .FirstOrDefaultAsync(x => x.Id == (long)id, cancellationToken)
            ?? throw new EntityNotFoundException(typeof(Vacation), id);
        
        return vacation;
    }
}