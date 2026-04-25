using ShiftTrack.Application.Modules.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Commands.UpdateShift;

public record UpdateShiftCommand(
    long Id,
    string Code,
    string Description,
    string Color,
    ShiftType Type,
    TimeSpan? StartTime,
    TimeSpan? EndTime) : IRequest<ShiftVm>;