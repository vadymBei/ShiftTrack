namespace ShiftTrack.Application.Features.Organization.Timesheet.Common.Dtos;

public record RestoreEmployeeShiftsDto(
    IEnumerable<long> EmployeeIds,
    DateTime StartDate,
    DateTime EndDate);