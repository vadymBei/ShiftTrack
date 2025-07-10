using ShiftTrack.Application.Features.Organization.Timesheet.Common.ViewModels;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Enums;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Shifts.Commands.CreateShift;

public record CreateShiftCommand(
    string Code,
    string Description,
    string Color,
    ShiftType Type,
    TimeSpan? StartTime,
    TimeSpan? EndTime) : IRequest<ShiftVm>;