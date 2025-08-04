using ShiftTrack.Application.Features.Booking.Common.Dtos;
using ShiftTrack.Data.Interfaces;
using ShiftTrack.Domain.Features.Booking.Vacations.Entities;

namespace ShiftTrack.Application.Features.Booking.Common.Interfaces;

public interface IVacationService : IEntityServiceBase<Vacation>
{
    Task<Vacation> ApproveVacation(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Vacation>>GetVacations(VacationsFilterDto filter, CancellationToken cancellationToken);
    Task<Vacation> RejectVacation(long id, CancellationToken cancellationToken);
}