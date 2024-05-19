using MediatR;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.DeletePosition
{
    public class DeletePositionCommand : IRequest
    {
        public long Id { get; set; }
    }
}
