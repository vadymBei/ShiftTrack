using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;

public interface IVacationService
{
    Task<Vacation> GetById(long id, CancellationToken cancellationToken);
    Task<Vacation> Approve(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Vacation>> GetFiltered(VacationsFilterDto filter, CancellationToken cancellationToken);
    Task<Vacation> Reject(long id, CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
}