using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Units.Queries.GetUnits;

public record GetUnitsQuery()
    : IRequest<IEnumerable<UnitVm>>;