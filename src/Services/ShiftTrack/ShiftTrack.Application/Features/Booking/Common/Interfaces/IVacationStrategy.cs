using ShiftTrack.Application.Features.Booking.Common.Dtos;
using ShiftTrack.Domain.Features.Booking.Vacations.Entities;

namespace ShiftTrack.Application.Features.Booking.Common.Interfaces;

public interface IVacationStrategy
{
    Task ApproveVacation(long id, CancellationToken cancellationToken);
    Task RejectVacation(long id, CancellationToken cancellationToken);
    Task<Vacation> GetVacationById(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Vacation>> GetVacations(VacationsFilterDto filter, CancellationToken cancellationToken);
}