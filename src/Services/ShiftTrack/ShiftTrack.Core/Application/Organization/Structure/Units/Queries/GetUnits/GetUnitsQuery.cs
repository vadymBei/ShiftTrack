using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnits
{
    public record GetUnitsQuery()
        : IRequest<IEnumerable<UnitVM>>;
}
