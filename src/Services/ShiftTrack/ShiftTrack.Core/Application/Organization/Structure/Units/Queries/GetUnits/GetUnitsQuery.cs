using ShiftTrack.Core.Application.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Core.Application.Organization.Structure.Units.Queries.GetUnits;

public record GetUnitsQuery()
    : IRequest<IEnumerable<UnitVM>>;