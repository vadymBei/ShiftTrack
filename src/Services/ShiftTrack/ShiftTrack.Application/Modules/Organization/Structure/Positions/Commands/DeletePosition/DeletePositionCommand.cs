using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.Commands.DeletePosition;

public record DeletePositionCommand(
    long Id) : IRequest;