using ShiftTrack.Application.Modules.Booking.Common.Dtos;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;

namespace ShiftTrack.Application.Modules.Booking.Common.Interfaces;

public interface ICommonVacationService
{
    IQueryable<Vacation> GetVacationsQuery(VacationsFilterDto filter);
    Task<Vacation> GetVacationById(long vacationId, CancellationToken cancellationToken);
    Task<Vacation> GetVacationForStatusChange(long vacationId, CancellationToken cancellationToken);
    Task SetVacationShifts(long vacationId, CancellationToken cancellationToken);
    Task RestoreEmployeeShiftsBeforeVacation(long vacationId, CancellationToken cancellationToken);
}