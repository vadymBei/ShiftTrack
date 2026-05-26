using ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Dtos;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Interfaces;

public interface IShiftService
{
    Task<Shift> Create(ShiftToCreateDto shiftToCreateDto, CancellationToken cancellationToken);
}