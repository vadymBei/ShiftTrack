namespace ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;

public record EmployeeShiftsByEmployeeIdDto(
    long EmployeeId,
    DateTime DateFrom,
    DateTime DateTo);