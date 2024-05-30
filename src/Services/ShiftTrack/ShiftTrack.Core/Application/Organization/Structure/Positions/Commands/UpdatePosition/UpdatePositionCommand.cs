using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.UpdatePosition
{
    public record UpdatePositionCommand(
        long Id,
        string Name,
        string Description) : IRequest<PositionVM>;
}
