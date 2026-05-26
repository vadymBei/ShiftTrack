using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Dtos;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Interfaces;

public interface IShiftRepository
{
    Task<Shift> GetById(long id, CancellationToken cancellationToken);
    Task<IEnumerable<Shift>> GetAll(CancellationToken cancellationToken);
    Task<IEnumerable<Shift>> GetShiftsByIds(IEnumerable<long> shiftIds, CancellationToken cancellationToken);
    Task<Shift> GetShiftByCode(string code, CancellationToken cancellationToken);
    Task CheckIfShiftExists(string code, CancellationToken cancellationToken);
    Task<Shift> Create(ShiftToCreateDto shiftToCreateDto, CancellationToken cancellationToken);
    Task Delete(long id, CancellationToken cancellationToken);
    Task<Shift> Update(ShiftToUpdateDto shiftToUpdateDto, CancellationToken cancellationToken);
}