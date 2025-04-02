using MediatR;
using ShiftTrack.Core.Application.Organization.Timesheet.Common.ViewModels.Shifts;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Enums;

namespace ShiftTrack.Core.Application.Organization.Timesheet.Shifts.Commands.CreateShift;

public record CreateShiftCommand(
    string Code,
    string Description,
    string Color,
    ShiftType Type) : IRequest<ShiftVM>;