using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Interfaces;

public interface IEmployeeShiftService
{
    Task<IEnumerable<EmployeeShift>> CreateEmployeeShifts(IEnumerable<EmployeeShiftToCreateDto> dtos, CancellationToken cancellationToken);
    Task<IEnumerable<EmployeeShift>> RestorePreviousEmployeeShifts(RestoreEmployeeShiftsDto dto, CancellationToken cancellationToken);
}   