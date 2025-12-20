using ShiftTrack.Application.Features.Booking.Common.Dtos;
using ShiftTrack.Domain.Features.Booking.Vacations.Entities;

namespace ShiftTrack.Application.Features.Booking.Common.Interfaces;

public interface ICommonVacationService
{
    IQueryable<Vacation> GetVacationsQuery(VacationsFilterDto filter);
    Task<Vacation> GetVacationById(long vacationId, CancellationToken cancellationToken);
    Task<Vacation> GetVacationForStatusChange(long vacationId, CancellationToken cancellationToken);
    Task SetVacationShifts(long vacationId, CancellationToken cancellationToken);
    Task RestoreEmployeeShiftsBeforeVacation(long vacationId, CancellationToken cancellationToken);
}