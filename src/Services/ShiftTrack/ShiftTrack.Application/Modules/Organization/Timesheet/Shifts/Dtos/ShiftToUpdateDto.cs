using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Enums;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Dtos;

public record ShiftToUpdateDto(
    long Id,
    string Code,
    string Description,
    string Color,
    ShiftType Type,
    TimeSpan? StartTime,
    TimeSpan? EndTime);