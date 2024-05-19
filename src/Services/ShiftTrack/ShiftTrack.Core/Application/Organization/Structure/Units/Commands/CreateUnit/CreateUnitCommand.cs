using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Commands.CreateUnit
{
    public class CreateUnitCommand : IRequest<UnitVM>
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }
    }
}
