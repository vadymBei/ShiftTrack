using ShiftTrack.Application.Modules.Booking.Vacations.Dtos;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;

namespace ShiftTrack.Application.Modules.Booking.Vacations.Interfaces;

public interface IVacationRepository
{
    Task<Vacation> Create(VacationToCreateDto vacationToCreateDto, CancellationToken cancellationToken);
    Task<Vacation> UpdateVacationStatus(UpdateVacationStatusDto dto, CancellationToken cancellationToken);
    Task<Vacation> GetById(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Vacation>> GetFiltered(VacationsFilterDto filter, CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
    Task<Vacation> Update(Vacation vacation, CancellationToken cancellationToken);
    Task<IEnumerable<Vacation>> GetByEmployeeOrAuthor(long employeeId, CancellationToken cancellationToken);
    Task<IEnumerable<Vacation>> GetForApproval(VacationApprovalFilterDto filter, int take, CancellationToken cancellationToken);
    Task<int> CountForApproval(VacationApprovalFilterDto filter, CancellationToken cancellationToken);
}