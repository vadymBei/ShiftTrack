using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Positions.Commands.CreatePosition
{
    public class CreatePositionCommand : IRequest<PositionVM>
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
