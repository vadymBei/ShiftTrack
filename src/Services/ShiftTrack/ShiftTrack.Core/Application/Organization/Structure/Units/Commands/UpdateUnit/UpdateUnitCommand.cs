using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Commands.UpdateUnit
{
    public class UpdateUnitCommand : IRequest<UnitVM>
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Code { get; set; }
    }
}
