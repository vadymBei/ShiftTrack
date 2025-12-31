using ShiftTrack.Application.Features.Organization.Structure.Common.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Features.Organization.Structure.Departments.Queries.GetDepartmentsByRoles;

public record GetDepartmentsByRolesQuery(
    long UnitId) : IRequest<IEnumerable<DepartmentVm>>;