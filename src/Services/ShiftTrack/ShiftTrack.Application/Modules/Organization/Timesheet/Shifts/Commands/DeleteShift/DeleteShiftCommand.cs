using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Timesheet.Shifts.Commands.DeleteShift;

public record DeleteShiftCommand(
    long Id) : IRequest;