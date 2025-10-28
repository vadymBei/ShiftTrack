namespace ShiftTrack.Application.Features.Organization.Timesheet.Common.Dtos;

public record TimesheetDto(
    DateTime Period,
    long DepartmentId);