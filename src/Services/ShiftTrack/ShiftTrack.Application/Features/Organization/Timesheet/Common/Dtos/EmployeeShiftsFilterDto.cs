namespace ShiftTrack.Application.Features.Organization.Timesheet.Common.Dtos;

public record EmployeeShiftsFilterDto(
    IEnumerable<long> EmployeeIds,
    DateTime StartDate,
    DateTime EndDate);