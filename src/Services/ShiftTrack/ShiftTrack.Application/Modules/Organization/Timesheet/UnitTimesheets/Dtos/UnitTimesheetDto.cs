namespace ShiftTrack.Application.Modules.Organization.Timesheet.UnitTimesheets.Dtos;

public record UnitTimesheetDto(
    DateTime Period,
    long DepartmentId);