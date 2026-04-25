using ShiftTrack.Application.Modules.Booking.Common.Dtos;
using ShiftTrack.Data.Interfaces;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;

namespace ShiftTrack.Application.Modules.Booking.Common.Interfaces;

public interface IVacationService : IEntityServiceBase<Vacation>
{
    Task<Vacation> ApproveVacation(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Vacation>>GetVacations(VacationsFilterDto filter, CancellationToken cancellationToken);
    Task<Vacation> RejectVacation(long id, CancellationToken cancellationToken);
}