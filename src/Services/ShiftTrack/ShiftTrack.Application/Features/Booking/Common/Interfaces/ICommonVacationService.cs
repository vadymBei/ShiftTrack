using ShiftTrack.Application.Features.Booking.Common.Dtos;
using ShiftTrack.Domain.Features.Booking.Vacations.Entities;

namespace ShiftTrack.Application.Features.Booking.Common.Interfaces;

public interface ICommonVacationService
{
    IQueryable<Vacation> GetVacations(VacationsFilterDto filter, CancellationToken cancellationToken);
    Task<Vacation> GetVacationById(long vacationId, CancellationToken cancellationToken);
    Task<Vacation> GetVacationForStatusChange(long vacationId, CancellationToken cancellationToken);
}