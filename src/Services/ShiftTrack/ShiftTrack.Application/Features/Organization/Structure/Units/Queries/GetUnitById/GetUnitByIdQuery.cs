using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Units.Queries.GetUnitById;

public record GetUnitByIdQuery(
    long Id) : IRequest<UnitVm>;