using ShiftTrack.Application.Modules.Organization.Structure.Units.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.UseCases.Queries.GetUnitById;

public record GetUnitByIdQuery(
    long Id) : IRequest<UnitVm>;