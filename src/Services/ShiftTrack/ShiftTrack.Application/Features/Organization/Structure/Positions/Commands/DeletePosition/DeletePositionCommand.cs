using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Positions.Commands.DeletePosition;

public record DeletePositionCommand(
    long Id) : IRequest;