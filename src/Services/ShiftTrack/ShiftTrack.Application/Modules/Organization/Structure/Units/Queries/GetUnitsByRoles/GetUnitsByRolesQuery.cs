using ShiftTrack.Application.Modules.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Units.Queries.GetUnitsByRoles;

public record GetUnitsByRolesQuery() : IRequest<IEnumerable<UnitVm>>;