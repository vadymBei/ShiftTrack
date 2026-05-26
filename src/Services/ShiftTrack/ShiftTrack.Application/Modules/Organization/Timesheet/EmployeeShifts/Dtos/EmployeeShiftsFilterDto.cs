namespace ShiftTrack.Application.Modules.Organization.Timesheet.EmployeeShifts.Dtos;

public record EmployeeShiftsFilterDto(
    IEnumerable<long> EmployeeIds,
    DateTime StartDate,
    DateTime EndDate,
    bool DisableTracking = false);