using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Timesheet.Shifts.Commands.DeleteShift;

public record DeleteShiftCommand(
    long Id) : IRequest;