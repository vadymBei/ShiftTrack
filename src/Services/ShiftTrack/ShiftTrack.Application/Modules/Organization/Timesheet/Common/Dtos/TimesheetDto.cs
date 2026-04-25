namespace ShiftTrack.Application.Modules.Organization.Timesheet.Common.Dtos;

public record TimesheetDto(
    DateTime Period,
    long DepartmentId);