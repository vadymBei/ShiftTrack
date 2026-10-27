using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;
using ShiftTrack.Domain.Modules.Booking.Vacations.Models;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;

public interface IVacationAccrualService
{
    /// <summary>Recalculates accrued vacation days of all employees.</summary>
    Task<int> RecalculateAll(CancellationToken cancellationToken);

    Task<VacationBalance> GetBalance(long employeeId, CancellationToken cancellationToken);

    /// <summary>First day the employee can start annual vacation (after 6 months of continuous work).</summary>
    Task<DateTime> GetAnnualVacationAvailableFrom(long employeeId, CancellationToken cancellationToken);

    /// <summary>Throws if annual vacation of the employee starts before 6 months of continuous work.</summary>
    Task EnsureCanTakeVacation(long employeeId, VacationType type, DateTime startDate,
        CancellationToken cancellationToken);
}
