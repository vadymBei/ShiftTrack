using MediatR;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.DeletePosition
{
    public record DeletePositionCommand(
        long Id) : IRequest;
}
