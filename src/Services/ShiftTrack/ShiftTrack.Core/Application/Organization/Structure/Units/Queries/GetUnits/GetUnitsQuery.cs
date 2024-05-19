using MediatR;
using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnits
{
    public class GetUnitsQuery : IRequest<IEnumerable<UnitVM>>
    {
    }
}
