using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Domain.Modules.Booking.Vacations.Enums;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;

public interface IVacationRepository
{
    Task<Vacation> Create(VacationToCreateDto vacationToCreateDto, CancellationToken cancellationToken);
    /// <summary>Tracked vacation with the employee and department, for changes made through its methods.</summary>
    Task<Vacation> GetForUpdate(long id, CancellationToken cancellationToken);

    /// <summary>Saves changes of entities loaded with <see cref="GetForUpdate"/>.</summary>
    Task Save(CancellationToken cancellationToken);
    Task<Vacation> GetById(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Vacation>> GetFiltered(VacationsFilterDto filter, CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
    Task<Vacation> Update(Vacation vacation, CancellationToken cancellationToken);
    Task<IEnumerable<Vacation>> GetByEmployeeOrAuthor(long employeeId, CancellationToken cancellationToken);
    Task<IEnumerable<Vacation>> GetForApproval(VacationApprovalFilterDto filter, int take, CancellationToken cancellationToken);
    Task<int> CountForApproval(VacationApprovalFilterDto filter, CancellationToken cancellationToken);

    /// <summary>Finally approved vacations of the given types; of one employee or of everyone.</summary>
    Task<IReadOnlyList<Vacation>> GetApproved(IReadOnlyCollection<VacationType> types, long? employeeId,
        CancellationToken cancellationToken);
}