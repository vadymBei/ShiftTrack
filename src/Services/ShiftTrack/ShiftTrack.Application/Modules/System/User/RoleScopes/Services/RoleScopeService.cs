using ShiftTrack.Application.Common.Interfaces;
using ShiftTrack.Application.Modules.Organization.Structure.Departments.Interfaces;
using ShiftTrack.Application.Modules.System.User.RoleScopes.Interfaces;
using ShiftTrack.Domain.Modules.System.User.EmployeeRoles.Models;

namespace ShiftTrack.Application.Modules.System.User.RoleScopes.Services;

public class RoleScopeService(
    ICurrentUserService currentUserService,
    IDepartmentRepository departmentRepository) : IRoleScopeService
{
    public RoleScope GetCurrentScope(string roleName) => RoleScope.Of(currentUserService.Employee, roleName);

    public async Task<IReadOnlySet<long>> GetCurrentDepartmentIds(string roleName, CancellationToken cancellationToken)
    {
        var scope = GetCurrentScope(roleName);
        var result = new HashSet<long>(scope.DepartmentIds);

        foreach (var unitId in scope.UnitIds)
        {
            var departments = await departmentRepository.GetDepartmentsByUnitId(unitId, cancellationToken);
            result.UnionWith(departments.Select(x => x.Id));
        }

        return result;
    }

    public async Task<IReadOnlySet<long>> GetCurrentUnitIds(string roleName, CancellationToken cancellationToken)
    {
        var scope = GetCurrentScope(roleName);
        var result = new HashSet<long>(scope.UnitIds);

        if (scope.DepartmentIds.Count > 0)
        {
            var departments = await departmentRepository.GetByIds(scope.DepartmentIds, cancellationToken);

            result.UnionWith(departments.Where(x => x.UnitId is not null).Select(x => x.UnitId!.Value));
        }

        return result;
    }
}
