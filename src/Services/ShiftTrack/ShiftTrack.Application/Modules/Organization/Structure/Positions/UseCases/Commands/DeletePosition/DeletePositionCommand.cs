using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Positions.UseCases.Commands.DeletePosition;

public record DeletePositionCommand(
    long Id) : IRequest;