using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Models;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;

public interface IVacationService
{
    Task<Vacation> GetById(long id, CancellationToken cancellationToken);
    Task<Vacation> Approve(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Vacation>> GetFiltered(VacationsFilterDto filter, CancellationToken cancellationToken);
    Task<Vacation> Reject(long id, CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
    Task<VacationBalance> GetVacationBalanceByEmployeeId(long employeeId, CancellationToken cancellationToken);
    Task<IEnumerable<Vacation>> GetMy(CancellationToken cancellationToken);
    Task<IEnumerable<Vacation>> GetPendingApprovals(int take, CancellationToken cancellationToken);
    Task<int> CountPendingApprovals(CancellationToken cancellationToken);
}