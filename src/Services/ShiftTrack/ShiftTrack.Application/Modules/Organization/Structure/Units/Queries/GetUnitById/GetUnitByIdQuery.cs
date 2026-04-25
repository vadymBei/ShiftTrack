using ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.Queries.GetUnitById;

public record GetUnitByIdQuery(
    long Id) : IRequest<UnitVm>;