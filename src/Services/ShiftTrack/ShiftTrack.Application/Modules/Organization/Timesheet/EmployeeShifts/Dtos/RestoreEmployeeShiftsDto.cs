namespace ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;

public record RestoreEmployeeShiftsDto(
    IEnumerable<long> EmployeeIds,
    DateTime StartDate,
    DateTime EndDate);