using MediatR;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Commands.DeleteUnit
{
    public record DeleteUnitCommand(
        long Id) : IRequest;
}
