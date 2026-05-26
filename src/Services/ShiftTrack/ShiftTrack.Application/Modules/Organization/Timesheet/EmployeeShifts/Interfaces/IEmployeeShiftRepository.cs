using ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Interfaces;

public interface IEmployeeShiftRepository
{
    Task Create(IEnumerable<EmployeeShift> employeeShifts, CancellationToken cancellationToken);
    Task Update(IEnumerable<EmployeeShift> employeeShifts, CancellationToken cancellationToken);
    Task<IEnumerable<EmployeeShift>> GetEmployeeShifts(EmployeeShiftsFilterDto dto, CancellationToken cancellationToken);
}