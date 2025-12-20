using ShiftTrack.Application.Features.Organization.Timesheet.Common.Dtos;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Common.Interfaces;

public interface IEmployeeShiftService
{
    Task<IEnumerable<EmployeeShift>> GetEmployeeShifts(EmployeeShiftsFilterDto filter, CancellationToken cancellationToken);
    Task<IEnumerable<EmployeeShift>> CreateEmployeeShifts(IEnumerable<EmployeeShiftToCreateDto> dtos, CancellationToken cancellationToken);
    Task<IEnumerable<EmployeeShift>> RestorePreviousEmployeeShifts(RestoreEmployeeShiftsDto dto, CancellationToken cancellationToken);
}   