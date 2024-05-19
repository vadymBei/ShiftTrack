using MediatR;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Commands.DeleteUnit
{
    public class DeleteUnitCommand : IRequest
    {
        public long Id { get; set; }
    }
}
