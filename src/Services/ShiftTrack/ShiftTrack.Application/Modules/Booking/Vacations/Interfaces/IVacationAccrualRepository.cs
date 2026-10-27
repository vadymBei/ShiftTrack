using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;

public interface IVacationAccrualRepository
{
    Task<IReadOnlyList<VacationAccrual>> GetByEmployeeId(long employeeId, CancellationToken cancellationToken);

    /// <summary>Replaces the accruals of the given employees with the new ones in one save.</summary>
    Task ReplaceForEmployees(IReadOnlyDictionary<long, IReadOnlyList<VacationAccrual>> accruals,
        CancellationToken cancellationToken);
}
