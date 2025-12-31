using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Units.Queries.GetUnitsByRoles;

public record GetUnitsByRolesQuery() : IRequest<IEnumerable<UnitVm>>;