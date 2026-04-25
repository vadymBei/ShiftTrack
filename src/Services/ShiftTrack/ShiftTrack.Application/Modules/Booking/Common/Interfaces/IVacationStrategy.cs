using ShiftTrack.Application.Modules.Booking.Common.Dtos;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;

namespace ShiftTrack.Application.Modules.Booking.Common.Interfaces;

public interface IVacationStrategy
{
    Task ApproveVacation(long id, CancellationToken cancellationToken);
    Task RejectVacation(long id, CancellationToken cancellationToken);
    Task<Vacation> GetVacationById(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Vacation>> GetVacations(VacationsFilterDto filter, CancellationToken cancellationToken);
}