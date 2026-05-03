using ShiftTrack.Application.Modules.Organization.Structure.Departments.ViewModels;
using ShiftTrack.Kernel.CQRS.Interfaces;

namespace ShiftTrack.Application.Modules.Organization.Structure.Departments.UseCases.Queries.GetDepartmentsByRoles;

public record GetDepartmentsByRolesQuery(
    long UnitId) : IRequest<IEnumerable<DepartmentVm>>;